using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintManager.Users.Application.Interfaces;
using PrintManager.Users.Application.Models.Requests;
using PrintManager.Users.Application.Models.Responses;
using PrintManager.Users.Application.Services;
using System.Security.Claims;

namespace PrintManager.Users.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ICompanyMemberService _companyMemberService;

        public CompaniesController(ICompanyService companyService, ICompanyMemberService companyMemberService)
        {
            _companyService = companyService;
            _companyMemberService = companyMemberService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(
            [FromBody] CreateCompanyRequest request)
        {
            var ownerUserId = User.FindFirstValue("sub")
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(ownerUserId))
                return Unauthorized();

            var response = await _companyService.CreateCompanyAsync(
                ownerUserId,
                request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("my")]
        [ProducesResponseType(typeof(ApiResponse<List<MyCompanyResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyCompanies()
        {
            var userId = User.FindFirstValue("sub")
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var response = await _companyService.GetMyCompaniesAsync(userId);

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
    }
}
