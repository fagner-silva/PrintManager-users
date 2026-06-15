using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PrintManager.Users.Domain.Entities;
using PrintManager.Users.Infra.Settings;

namespace PrintManager.Users.Infra.Context
{
    public class MongoContext
    {
        public IMongoCollection<User> Users { get; }

        public IMongoCollection<Company> Companies { get; }

        public IMongoCollection<CompanyMember> CompanyMembers { get; }

        public MongoContext(IOptions<MongoDbSettings> settings)
        {
            var mongoClient =
                new MongoClient(settings.Value.ConnectionString);

            var database =
                mongoClient.GetDatabase(settings.Value.DatabaseName);

            Users = database.GetCollection<User>(
                settings.Value.UsersCollectionName);

            Companies = database.GetCollection<Company>(
                settings.Value.CompaniesCollectionName);

            CompanyMembers = database.GetCollection<CompanyMember>(
                settings.Value.CompanyMembersCollectionName);
        }
    }
}
