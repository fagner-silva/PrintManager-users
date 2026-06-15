using PrintManager.Users.Application.Interfaces;
using PrintManager.Users.Application.Models.Requests;
using PrintManager.Users.Application.Models.Responses;
using PrintManager.Users.Domain.Entities;
using PrintManager.Users.Domain.Enums;
using PrintManager.Users.Domain.Interfaces;


namespace PrintManager.Users.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IUserValidator _userValidator;
        private readonly IUserMapper _userMapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyMemberRepository _companyMemberRepository;
        public UserService(
    IUserRepository userRepository,
    ICompanyRepository companyRepository,
    ICompanyMemberRepository companyMemberRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUserValidator userValidator,
    IUserMapper userMapper)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _companyMemberRepository = companyMemberRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _userValidator = userValidator;
            _userMapper = userMapper;
        }

        public async Task<ApiResponse<UserResponse>> RegisterAsync(RegisterUserRequest request)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                return new ApiResponse<UserResponse>
                {
                    Success = false,
                    Message = "E-mail já cadastrado.",
                    Errors = new List<string> { "Já existe um usuário com este e-mail." }
                };
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email.ToLower().Trim(),
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                IsActive = true,
                IsBlocked = false,
                CreatedAt = DateTime.UtcNow
            };
            await _userRepository.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(request.CompanyName))
            {
                var company = new Company
                {
                    Name = request.CompanyName.Trim(),
                    OwnerUserId = user.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var companyMember = new CompanyMember
                {
                    CompanyId = company.Id,
                    UserId = user.Id,
                    Role = UserRole.Admin,
                    IsActive = true,
                    IsBlocked = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _companyRepository.CreateAsync(company);
                await _companyMemberRepository.CreateAsync(companyMember);
            }

            return new ApiResponse<UserResponse>
            {
                Success = true,
                Message = "Usuário cadastrado com sucesso.",
                Data = _userMapper.MapToUserResponse(user)
            };
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            var validationResult = _userValidator.ValidateLogin(user, request.Password);

            if (!validationResult.Success)
                return validationResult;

            var token = _jwtService.GenerateToken(user!);
            return BuildLoginSuccessResponse(user!, token);
        }

        private ApiResponse<LoginResponse> BuildLoginSuccessResponse(User user, string token)
        {
            return new ApiResponse<LoginResponse>
            {
                Success = true,
                Message = "Login realizado com sucesso.",
                Data = new LoginResponse
                {
                    Token = token,
                    TokenType = "Bearer",
                    ExpiresIn = 28800
                }
            };
        }

        public async Task<ApiResponse<MeResponse>> GetMeAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return new ApiResponse<MeResponse>
                {
                    Success = false,
                    Message = "Usuário não encontrado.",
                    Errors = ["Usuário não encontrado."]
                };
            }

            var memberships = await _companyMemberRepository.GetByUserIdAsync(userId);
            var activeMemberships = memberships
    .Where(member => member.IsActive && !member.IsBlocked)
    .ToList();

            var companyIds = activeMemberships.Select(member => member.CompanyId);

            var companies = await _companyRepository.GetByIdsAsync(companyIds);

            var companyResponses = activeMemberships.Select(member =>
            {
                var company = companies.FirstOrDefault(c => c.Id == member.CompanyId);

                return new UserCompanyResponse
                {
                    CompanyId = member.CompanyId,
                    CompanyName = company?.Name ?? string.Empty,
                    Role = member.Role.ToString()
                };
            }).ToList();

            return new ApiResponse<MeResponse>
            {
                Success = true,
                Message = "Usuário autenticado encontrado com sucesso.",
                Data = new MeResponse
                {
                    User = _userMapper.MapToUserResponse(user),
                    Companies = companyResponses
                }
            };
        }

        public async Task<ApiResponse<object>> ChangePasswordAsync(
    string userId,
    ChangePasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Usuário não encontrado.",
                    Errors = ["Usuário não encontrado."]
                };
            }

            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Senha atual inválida.",
                    Errors = ["A senha atual informada não confere."]
                };
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Confirmação de senha inválida.",
                    Errors = ["A nova senha e a confirmação precisam ser iguais."]
                };
            }

            user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Senha alterada com sucesso."
            };
        }


    }
}
