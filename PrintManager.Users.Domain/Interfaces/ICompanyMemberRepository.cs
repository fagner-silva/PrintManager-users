using PrintManager.Users.Domain.Entities;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManager.Users.Domain.Interfaces
{
    public interface ICompanyMemberRepository
    {
        Task CreateAsync(CompanyMember member);
        Task<IEnumerable<CompanyMember>> GetByUserIdAsync(string userId);
        Task<CompanyMember?> GetByUserAndCompanyAsync(string userId, string companyId);
        Task UpdateAsync(CompanyMember member);
        Task<IEnumerable<CompanyMember>> GetByCompanyIdAsync(string companyId);

    }
}
