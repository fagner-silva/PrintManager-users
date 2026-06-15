using PrintManager.Users.Application.Interfaces;
using PrintManager.Users.Application.Models.Requests;
using PrintManager.Users.Application.Models.Responses;
using PrintManager.Users.Domain.Entities;
using PrintManager.Users.Domain.Enums;
using PrintManager.Users.Domain.Interfaces;


namespace PrintManager.Users.Application.Services
{
    public class CompanyService : ICompanyService
    {

        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyMemberRepository _companyMemberRepository;
        private readonly IUserRepository _userRepository;

        public CompanyService(
    ICompanyRepository companyRepository,
    ICompanyMemberRepository companyMemberRepository,
    IUserRepository userRepository)
        {
            _companyRepository = companyRepository;
            _companyMemberRepository = companyMemberRepository;
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<object>> CreateCompanyAsync(
    string ownerUserId,
    CreateCompanyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Nome da empresa obrigatório.",
                    Errors = ["Informe o nome da empresa."]
                };
            }

            var owner = await _userRepository.GetByIdAsync(ownerUserId);

            if (owner is null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Usuário não encontrado.",
                    Errors = ["Usuário autenticado não encontrado."]
                };
            }

            var company = new Company
            {
                Name = request.Name.Trim(),
                OwnerUserId = ownerUserId,
                CpfCnpj = request.CpfCnpj?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var member = new CompanyMember
            {
                CompanyId = company.Id,
                UserId = ownerUserId,
                Role = UserRole.Admin,
                IsActive = true,
                IsBlocked = false,
                CreatedAt = DateTime.UtcNow
            };

            await _companyRepository.CreateAsync(company);
            await _companyMemberRepository.CreateAsync(member);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Empresa criada com sucesso."
            };
        }
        public async Task<ApiResponse<List<MyCompanyResponse>>> GetMyCompaniesAsync(string userId)
        {
            var memberships = await _companyMemberRepository.GetByUserIdAsync(userId);

            var activeMemberships = memberships
                .Where(member => member.IsActive && !member.IsBlocked)
                .ToList();

            if (!activeMemberships.Any())
            {
                return new ApiResponse<List<MyCompanyResponse>>
                {
                    Success = true,
                    Message = "Nenhuma empresa encontrada para este usuário.",
                    Data = []
                };
            }

            var companyIds = activeMemberships.Select(member => member.CompanyId);

            var companies = await _companyRepository.GetByIdsAsync(companyIds);

            var response = activeMemberships.Select(member =>
            {
                var company = companies.FirstOrDefault(c => c.Id == member.CompanyId);

                return new MyCompanyResponse
                {
                    CompanyId = member.CompanyId,
                    CompanyName = company?.Name ?? string.Empty,
                    CpfCnpj = company?.CpfCnpj,
                    Role = member.Role.ToString(),
                    IsOwner = company?.OwnerUserId == userId
                };
            }).ToList();

            return new ApiResponse<List<MyCompanyResponse>>
            {
                Success = true,
                Message = "Empresas recuperadas com sucesso.",
                Data = response
            };
        }
    }
}
