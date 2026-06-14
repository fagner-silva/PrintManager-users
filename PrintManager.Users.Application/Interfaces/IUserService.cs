using PrintManager.Users.Application.Models.Requests;
using PrintManager.Users.Application.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManager.Users.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserResponse>> RegisterAsync(RegisterUserRequest request);
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    }
}
