using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrintManager.Users.Domain.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string CpfCnpj { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; } = false;
        public int LoginAttempts { get; set; }
        public DateTime? BlockedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
