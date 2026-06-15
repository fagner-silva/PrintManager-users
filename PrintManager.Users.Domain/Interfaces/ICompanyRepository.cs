using PrintManager.Users.Domain.Entities;

namespace PrintManager.Users.Domain.Interfaces
{
    public interface ICompanyRepository
    {
        Task CreateAsync(Company company);
        Task<Company?> GetByIdAsync(string id);
        Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<string> ids);
    }
}
