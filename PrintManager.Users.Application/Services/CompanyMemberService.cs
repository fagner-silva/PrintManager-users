using PrintManager.Users.Application.Interfaces;
using PrintManager.Users.Application.Models.Requests;
using PrintManager.Users.Application.Models.Responses;
using PrintManager.Users.Domain.Entities;
using PrintManager.Users.Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PrintManager.Users.Application.Services
{
    public class CompanyMemberService : ICompanyMemberService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyMemberRepository _companyMemberRepository;
        private readonly IUserRepository _userRepository;

        public CompanyMemberService(
    ICompanyRepository companyRepository,
    ICompanyMemberRepository companyMemberRepository,
    IUserRepository userRepository)
        {
            _companyRepository = companyRepository;
            _companyMemberRepository = companyMemberRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<object>> BlockMemberAsync(
            string ownerUserId,
            string companyId,
            string targetUserId)
        {
            var validation = await ValidateOwnerPermissionAsync(
                ownerUserId,
                companyId,
                targetUserId);

            if (!validation.Success)
                return validation;

            var targetMember = await _companyMemberRepository
                .GetByUserAndCompanyAsync(targetUserId, companyId);

            targetMember!.IsBlocked = true;
            targetMember.BlockedByUserId = ownerUserId;
            targetMember.BlockedAt = DateTime.UtcNow;

            await _companyMemberRepository.UpdateAsync(targetMember);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Membro bloqueado com sucesso."
            };
        }

        public async Task<ApiResponse<object>> UnblockMemberAsync(
            string ownerUserId,
            string companyId,
            string targetUserId)
        {
            var validation = await ValidateOwnerPermissionAsync(
                ownerUserId,
                companyId,
                targetUserId);

            if (!validation.Success)
                return validation;

            var targetMember = await _companyMemberRepository
                .GetByUserAndCompanyAsync(targetUserId, companyId);

            targetMember!.IsBlocked = false;
            targetMember.BlockedByUserId = null;
            targetMember.BlockedAt = null;

            await _companyMemberRepository.UpdateAsync(targetMember);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Membro desbloqueado com sucesso."
            };
        }

        private async Task<ApiResponse<object>> ValidateOwnerPermissionAsync(
            string ownerUserId,
            string companyId,
            string targetUserId)
        {
            if (ownerUserId == targetUserId)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Operação inválida.",
                    Errors = ["Você não pode bloquear ou desbloquear a si mesmo."]
                };
            }

            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company is null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Empresa não encontrada.",
                    Errors = ["Empresa não encontrada."]
                };
            }

            if (company.OwnerUserId != ownerUserId)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Acesso negado.",
                    Errors = ["Somente o owner da empresa pode executar esta ação."]
                };
            }

            var targetMember = await _companyMemberRepository
                .GetByUserAndCompanyAsync(targetUserId, companyId);

            if (targetMember is null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Membro não encontrado.",
                    Errors = ["Este usuário não pertence a esta empresa."]
                };
            }

            return new ApiResponse<object>
            {
                Success = true
            };
        }
        public async Task<ApiResponse<object>> AddMemberAsync(
    string ownerUserId,
    string companyId,
    AddCompanyMemberRequest request)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company is null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Empresa não encontrada.",
                    Errors = ["Empresa não encontrada."]
                };
            }

            if (company.OwnerUserId != ownerUserId)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Acesso negado.",
                    Errors = ["Somente o owner pode adicionar membros."]
                };
            }

            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Usuário não encontrado.",
                    Errors = ["O usuário precisa estar cadastrado antes de ser adicionado à empresa."]
                };
            }

            var existingMember = await _companyMemberRepository
    .GetByUserAndCompanyAsync(user.Id, companyId);

            if (existingMember is not null)
            {
                if (existingMember.IsActive)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Usuário já pertence à empresa.",
                        Errors = ["Este usuário já é membro desta empresa."]
                    };
                }

                existingMember.IsActive = true;
                existingMember.IsBlocked = false;
                existingMember.Role = request.Role;
                existingMember.UpdatedAt = DateTime.UtcNow;

                await _companyMemberRepository.UpdateAsync(existingMember);

                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Membro reativado com sucesso."
                };
            }

            var member = new CompanyMember
            {
                CompanyId = companyId,
                UserId = user.Id,
                Role = request.Role,
                IsActive = true,
                IsBlocked = false,
                CreatedAt = DateTime.UtcNow
            };

            await _companyMemberRepository.CreateAsync(member);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Membro adicionado com sucesso."
            };
        }
        public async Task<ApiResponse<List<CompanyMemberResponse>>> GetMembersAsync(
    string requesterUserId,
    string companyId)
        {
            var requesterMember = await _companyMemberRepository
                .GetByUserAndCompanyAsync(requesterUserId, companyId);

            if (requesterMember is null || !requesterMember.IsActive || requesterMember.IsBlocked)
            {
                return new ApiResponse<List<CompanyMemberResponse>>
                {
                    Success = false,
                    Message = "Acesso negado.",
                    Errors = ["Você não pertence a esta empresa ou está bloqueado."]
                };
            }

            var memberships = await _companyMemberRepository.GetByCompanyIdAsync(companyId);

            var activeMemberships = memberships
                .Where(member => member.IsActive)
                .ToList();

            var userIds = activeMemberships.Select(member => member.UserId);

            var users = await _userRepository.GetByIdsAsync(userIds);

            var company = await _companyRepository.GetByIdAsync(companyId);

            var response = activeMemberships.Select(member =>
            {
                var user = users.FirstOrDefault(u => u.Id == member.UserId);

                return new CompanyMemberResponse
                {
                    UserId = member.UserId,
                    Name = user?.Name ?? string.Empty,
                    Email = user?.Email ?? string.Empty,
                    Role = member.Role.ToString(),
                    IsOwner = company?.OwnerUserId == member.UserId,
                    IsActive = member.IsActive,
                    IsBlocked = member.IsBlocked
                };
            }).ToList();

            return new ApiResponse<List<CompanyMemberResponse>>
            {
                Success = true,
                Message = "Membros recuperados com sucesso.",
                Data = response
            };
        }
        public async Task<ApiResponse<object>> UpdateMemberRoleAsync(
    string ownerUserId,
    string companyId,
    string targetUserId,
    UpdateCompanyMemberRoleRequest request)
        {
            if (ownerUserId == targetUserId)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Operação inválida.",
                    Errors = ["Você não pode alterar o próprio perfil."]
                };
            }

            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company is null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Empresa não encontrada.",
                    Errors = ["Empresa não encontrada."]
                };
            }

            if (company.OwnerUserId != ownerUserId)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Acesso negado.",
                    Errors = ["Somente o owner da empresa pode alterar perfis."]
                };
            }

            var targetMember = await _companyMemberRepository
                .GetByUserAndCompanyAsync(targetUserId, companyId);

            if (targetMember is null || !targetMember.IsActive)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Membro não encontrado.",
                    Errors = ["Este usuário não pertence a esta empresa."]
                };
            }

            if (targetMember.UserId == company.OwnerUserId)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Operação inválida.",
                    Errors = ["Não é possível alterar o perfil do owner da empresa."]
                };
            }

            targetMember.Role = request.Role;
            targetMember.UpdatedAt = DateTime.UtcNow;

            await _companyMemberRepository.UpdateAsync(targetMember);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Perfil do membro atualizado com sucesso."
            };
        }
        public async Task<ApiResponse<object>> RemoveMemberAsync(
    string ownerUserId,
    string companyId,
    string targetUserId)
        {
            if (ownerUserId == targetUserId)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Operação inválida.",
                    Errors = ["Você não pode remover a si mesmo da empresa."]
                };
            }

            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company is null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Empresa não encontrada.",
                    Errors = ["Empresa não encontrada."]
                };
            }

            if (company.OwnerUserId != ownerUserId)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Acesso negado.",
                    Errors = ["Somente o owner da empresa pode remover membros."]
                };
            }

            if (targetUserId == company.OwnerUserId)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Operação inválida.",
                    Errors = ["Não é possível remover o owner da empresa."]
                };
            }

            var targetMember = await _companyMemberRepository
                .GetByUserAndCompanyAsync(targetUserId, companyId);

            if (targetMember is null || !targetMember.IsActive)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Membro não encontrado.",
                    Errors = ["Este usuário não pertence a esta empresa."]
                };
            }

            targetMember.IsActive = false;
            targetMember.IsBlocked = false;
            targetMember.UpdatedAt = DateTime.UtcNow;

            await _companyMemberRepository.UpdateAsync(targetMember);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Membro removido com sucesso."
            };
        }
    }
}
