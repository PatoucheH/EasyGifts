using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string? IdentityId { get; set; }
        public string? Username { get; set; }
        public required string Email { get; set; }
        public List<Gift> Gifts { get; set; } = new();
    }
}
