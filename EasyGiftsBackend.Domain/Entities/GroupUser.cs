using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.Entities
{
    public class GroupUser
    {
        public Guid GroupId { get; set; }
        public required Group Group { get; set; }
        public Guid UserId { get; set; }
        public required User User { get; set; }
    }
}
