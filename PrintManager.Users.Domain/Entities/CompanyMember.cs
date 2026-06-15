using PrintManager.Users.Domain.Enums;

namespace PrintManager.Users.Domain.Entities
{
    public  class CompanyMember
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CompanyId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Operator;
        public bool IsBlocked { get; set; }
        public string? BlockedByUserId { get; set; }
        public DateTime? BlockedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
