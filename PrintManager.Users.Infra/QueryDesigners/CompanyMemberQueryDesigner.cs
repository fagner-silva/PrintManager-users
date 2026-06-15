using MongoDB.Driver;
using PrintManager.Users.Domain.Entities;


namespace PrintManager.Users.Infra.QueryDesigners
{
    public class CompanyMemberQueryDesigner
    {
        public static FilterDefinition<CompanyMember> ByUserId(string userId)
        {
            return Builders<CompanyMember>.Filter.Eq(member => member.UserId, userId);
        }

        public static FilterDefinition<CompanyMember> ByUserAndCompany(string userId, string companyId)
        {
            return Builders<CompanyMember>.Filter.And(
                Builders<CompanyMember>.Filter.Eq(member => member.UserId, userId),
                Builders<CompanyMember>.Filter.Eq(member => member.CompanyId, companyId)
            );
        }
        public static FilterDefinition<CompanyMember> ByCompanyId(string companyId)
        {
            return Builders<CompanyMember>.Filter.Eq(member => member.CompanyId, companyId);
        }
    }
}
