using PrintManager.Users.Application.Models.Requests;
using PrintManager.Users.Application.Models.Responses;
namespace PrintManager.Users.Application.Interfaces
{
    public interface ICompanyMemberService
    {
        Task<ApiResponse<object>> BlockMemberAsync(
       string ownerUserId,
       string companyId,
       string targetUserId);

        Task<ApiResponse<object>> UnblockMemberAsync(
            string ownerUserId,
            string companyId,
            string targetUserId);

        Task<ApiResponse<object>> AddMemberAsync(
    string ownerUserId,
    string companyId,
    AddCompanyMemberRequest request);

        Task<ApiResponse<List<CompanyMemberResponse>>> GetMembersAsync(
    string requesterUserId,
    string companyId);
        Task<ApiResponse<object>> UpdateMemberRoleAsync(string ownerUserId, string companyId, string targetUserId, UpdateCompanyMemberRoleRequest request);

        Task<ApiResponse<object>> RemoveMemberAsync(
    string ownerUserId,
    string companyId,
    string targetUserId);

    }

}
