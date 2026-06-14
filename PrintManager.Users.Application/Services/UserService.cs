using PrintManager.Users.Application.Interfaces;
using PrintManager.Users.Application.Models.Requests;
using PrintManager.Users.Application.Models.Responses;
using PrintManager.Users.Domain.Entities;
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
        public UserService(
       IUserRepository userRepository,
       IPasswordHasher passwordHasher,
       IJwtService jwtService,
       IUserValidator userValidator,
       IUserMapper userMapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _userValidator = userValidator;
            _userMapper = userMapper;
        }

        public async Task<ApiResponse<UserResponse>> RegisterAsync(RegisterUserRequest request)
        {
            var userExists = await _userRepository.GetByEmailAsync(request.Email);

            if (userExists is not null)
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
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Role = request.Role,
                IsActive = true,
                IsBlocked = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

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
            return BuildLoginSuccessResponse(user, token);
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
                    User = _userMapper.MapToUserResponse(user)
                }
            };
        }

        
        
    }
}
