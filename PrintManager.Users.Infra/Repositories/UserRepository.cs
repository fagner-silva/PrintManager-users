using MongoDB.Driver;
using PrintManager.Users.Domain.Entities;
using PrintManager.Users.Domain.Interfaces;
using PrintManager.Users.Infra.Context;
using PrintManager.Users.Infra.QueryDesigners;

namespace PrintManager.Users.Infra.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(MongoContext context)
    {
        _users = context.Users;
    }

    public async Task CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _users
            .Find(UserQueryDesigner.ById(id))
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _users
            .Find(UserQueryDesigner.ByEmail(email))
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _users
            .Find(Builders<User>.Filter.Empty)
            .ToListAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _users
            .Find(UserQueryDesigner.ByEmail(email))
            .AnyAsync();
    }

    public async Task UpdateAsync(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;

        await _users.ReplaceOneAsync(
            UserQueryDesigner.ById(user.Id),
            user);
    }
    public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<string> ids)
    {
        return await _users
            .Find(Builders<User>.Filter.In(user => user.Id, ids))
            .ToListAsync();
    }
}