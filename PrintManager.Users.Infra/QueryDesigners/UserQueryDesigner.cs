using MongoDB.Driver;
using PrintManager.Users.Domain.Entities;
using PrintManager.Users.Domain.Enums;

namespace PrintManager.Users.Infra.QueryDesigners
{
    public class UserQueryDesigner
    {
        public static FilterDefinition<User> ById(string id)
        {
            return Builders<User>.Filter.Eq(user => user.Id, id);
        }

        public static FilterDefinition<User> ByEmail(string email)
        {
            return Builders<User>.Filter.Eq(user => user.Email, email.ToLower().Trim());
        }

        public static FilterDefinition<User> Active()
        {
            return Builders<User>.Filter.Eq(user => user.IsActive, true);
        }

        public static FilterDefinition<User> Blocked()
        {
            return Builders<User>.Filter.Eq(user => user.IsBlocked, true);
        }

        public static FilterDefinition<User> ByRole(UserRole role)
        {
            return Builders<User>.Filter.Eq(user => user.Role, role);
        }
        public static FilterDefinition<User> ActiveByEmail(string email)
        {
            return Builders<User>.Filter.And(
                ByEmail(email),
                Active()
            );
        }
    }
}
