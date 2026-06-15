using MongoDB.Driver;
using PrintManager.Users.Domain.Entities;
namespace PrintManager.Users.Infra.QueryDesigners
{
    public class CompanyQueryDesigner
    {
        public static FilterDefinition<Company> ById(string id)
        {
            return Builders<Company>.Filter.Eq(company => company.Id, id);
        }
    }
}
