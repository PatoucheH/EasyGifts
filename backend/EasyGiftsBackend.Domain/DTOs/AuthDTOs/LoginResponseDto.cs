using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.DTOs.AuthDTOs
{
    public class LoginResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? Token { get; set; }
    }
}
