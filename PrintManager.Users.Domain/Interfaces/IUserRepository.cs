using PrintManager.Users.Domain.Entities;

namespace PrintManager.Users.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<bool> ExistsByEmailAsync(string email);
    Task UpdateAsync(User user);
    Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<string> ids);
}