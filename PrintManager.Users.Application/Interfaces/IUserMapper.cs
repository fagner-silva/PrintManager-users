using PrintManager.Users.Application.Models.Responses;
using PrintManager.Users.Domain.Entities;

namespace PrintManager.Users.Application.Interfaces
{
    public interface IUserMapper
    {
        UserResponse MapToUserResponse(User user);
    }
}
