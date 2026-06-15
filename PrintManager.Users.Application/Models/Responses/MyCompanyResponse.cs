

namespace PrintManager.Users.Application.Models.Responses
{
    public class MyCompanyResponse
    {
        public string CompanyId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? CpfCnpj { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsOwner { get; set; }
    }
}
