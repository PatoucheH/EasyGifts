using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.DTOs.AuthDTOs
{
    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
