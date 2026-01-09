using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<GroupUser> GroupUsers { get; set; } = new();
        public required Guid AdminId { get; set; }
        public required User Admin { get; init; }
    }
}
