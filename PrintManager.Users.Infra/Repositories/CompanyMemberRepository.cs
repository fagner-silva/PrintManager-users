using MongoDB.Driver;
using PrintManager.Users.Domain.Entities;
using PrintManager.Users.Domain.Interfaces;
using PrintManager.Users.Infra.Context;
using PrintManager.Users.Infra.QueryDesigners;


namespace PrintManager.Users.Infra.Repositories
{
    public class CompanyMemberRepository : ICompanyMemberRepository
    {
        private readonly IMongoCollection<CompanyMember> _companyMembers;

        public CompanyMemberRepository(MongoContext context)
        {
            _companyMembers = context.CompanyMembers;
        }

        public async Task CreateAsync(CompanyMember member)
        {
            await _companyMembers.InsertOneAsync(member);
        }

        public async Task<IEnumerable<CompanyMember>> GetByUserIdAsync(string userId)
        {
            return await _companyMembers
                .Find(CompanyMemberQueryDesigner.ByUserId(userId))
                .ToListAsync();
        }

        public async Task<CompanyMember?> GetByUserAndCompanyAsync(string userId, string companyId)
        {
            return await _companyMembers
                .Find(CompanyMemberQueryDesigner.ByUserAndCompany(userId, companyId))
                .FirstOrDefaultAsync();
        }
        public async Task UpdateAsync(CompanyMember member)
        {
            member.UpdatedAt = DateTime.UtcNow;

            await _companyMembers.ReplaceOneAsync(
                CompanyMemberQueryDesigner.ByUserAndCompany(member.UserId, member.CompanyId),
                member);
        }
        public async Task<IEnumerable<CompanyMember>> GetByCompanyIdAsync(string companyId)
        {
            return await _companyMembers
                .Find(CompanyMemberQueryDesigner.ByCompanyId(companyId))
                .ToListAsync();
        }
    }
}
