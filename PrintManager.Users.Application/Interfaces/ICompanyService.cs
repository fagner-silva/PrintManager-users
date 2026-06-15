using PrintManager.Users.Application.Models.Requests;
using PrintManager.Users.Application.Models.Responses;

namespace PrintManager.Users.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<ApiResponse<object>> CreateCompanyAsync(string ownerUserId,CreateCompanyRequest request);
        Task<ApiResponse<List<MyCompanyResponse>>> GetMyCompaniesAsync(string userId);
    }
}
