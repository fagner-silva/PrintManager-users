using PrintManager.Users.Domain.Enums;

namespace PrintManager.Users.Application.Models.Requests
{
    public class RegisterUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Operator;
    }
}
