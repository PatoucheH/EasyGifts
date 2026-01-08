using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.Entities
{
    public class GroupUser
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
