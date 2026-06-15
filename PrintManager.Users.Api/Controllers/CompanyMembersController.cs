using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintManager.Users.Application.Interfaces;
using PrintManager.Users.Application.Models.Responses;
using System.Security.Claims;
using PrintManager.Users.Application.Models.Requests;
namespace PrintManager.Users.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/companies/{companyId}/members")]
    public class CompanyMembersController : ControllerBase
    {
        private readonly ICompanyMemberService _companyMemberService;

        public CompanyMembersController(ICompanyMemberService companyMemberService)
        {
            _companyMemberService = companyMemberService;
        }

        [HttpPatch("{targetUserId}/block")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BlockMember(
            string companyId,
            string targetUserId)
        {
            var ownerUserId = User.FindFirstValue("sub")
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(ownerUserId))
                return Unauthorized();

            var response = await _companyMemberService.BlockMemberAsync(
                ownerUserId,
                companyId,
                targetUserId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPatch("{targetUserId}/unblock")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UnblockMember(
            string companyId,
            string targetUserId)
        {
            var ownerUserId = User.FindFirstValue("sub")
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(ownerUserId))
                return Unauthorized();

            var response = await _companyMemberService.UnblockMemberAsync(
                ownerUserId,
                companyId,
                targetUserId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddMember(
    string companyId,
    [FromBody] AddCompanyMemberRequest request)
        {
            var ownerUserId = User.FindFirstValue("sub")
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(ownerUserId))
                return Unauthorized();

            var response = await _companyMemberService.AddMemberAsync(
                ownerUserId,
                companyId,
                request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<CompanyMemberResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMembers(string companyId)
        {
            var requesterUserId = User.FindFirstValue("sub")
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(requesterUserId))
                return Unauthorized();

            var response = await _companyMemberService.GetMembersAsync(
                requesterUserId,
                companyId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpPatch("{targetUserId}/role")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMemberRole(
    string companyId,
    string targetUserId,
    [FromBody] UpdateCompanyMemberRoleRequest request)
        {
            var ownerUserId = User.FindFirstValue("sub")
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(ownerUserId))
                return Unauthorized();

            var response = await _companyMemberService.UpdateMemberRoleAsync(
                ownerUserId,
                companyId,
                targetUserId,
                request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpDelete("{targetUserId}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveMember(
    string companyId,
    string targetUserId)
        {
            var ownerUserId = User.FindFirstValue("sub")
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(ownerUserId))
                return Unauthorized();

            var response = await _companyMemberService.RemoveMemberAsync(
                ownerUserId,
                companyId,
                targetUserId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

    }
}
