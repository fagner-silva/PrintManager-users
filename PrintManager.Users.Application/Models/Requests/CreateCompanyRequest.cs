namespace PrintManager.Users.Application.Models.Requests
{
    public class CreateCompanyRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? CpfCnpj { get; set; } = string.Empty;
    }
}
