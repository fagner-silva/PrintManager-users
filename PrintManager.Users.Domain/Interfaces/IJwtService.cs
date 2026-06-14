using PrintManager.Users.Domain.Entities;

namespace PrintManager.Users.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
