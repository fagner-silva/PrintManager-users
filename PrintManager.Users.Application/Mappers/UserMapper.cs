using PrintManager.Users.Application.Models.Responses;
using PrintManager.Users.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManager.Users.Application.Mappers
{
    using PrintManager.Users.Application.Interfaces;

    public class UserMapper : IUserMapper
    {
        public UserResponse MapToUserResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsActive = user.IsActive,
                IsBlocked = user.IsBlocked
            };
        }
    }
}
