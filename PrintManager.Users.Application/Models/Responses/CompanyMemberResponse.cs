using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManager.Users.Application.Models.Responses
{
    public class CompanyMemberResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsOwner { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
    }
}
