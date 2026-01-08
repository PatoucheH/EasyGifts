using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.DTOs.GroupDTOs
{
    public class InviteUserRequest
    {
        public string Email { get; set; } = null!;
    }
}
