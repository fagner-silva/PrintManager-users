
namespace PrintManager.Users.Application.Models.Responses
{
    public class MeResponse
    {
        public UserResponse User { get; set; } = new();
        public List<UserCompanyResponse> Companies { get; set; } = [];
    }
}
