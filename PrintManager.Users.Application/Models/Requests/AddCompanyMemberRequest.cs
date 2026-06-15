using PrintManager.Users.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManager.Users.Application.Models.Requests
{
    public class AddCompanyMemberRequest
    {
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Operator;
    }
}
