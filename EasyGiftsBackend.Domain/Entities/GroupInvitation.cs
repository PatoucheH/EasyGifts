using System;

namespace EasyGiftsBackend.Domain.Entities
{
    public class GroupInvitation
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool Accepted { get; set; }
    }
}
