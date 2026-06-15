using PrintManager.Users.Domain.Enums;


namespace PrintManager.Users.Application.Models.Requests
{
    public class UpdateCompanyMemberRoleRequest
    {
        public UserRole Role { get; set; }
    }
}
