using EasyGiftsBackend.Domain.DTOs;
using EasyGiftsBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Application.Interfaces
{
    public interface IAuthService
    {
        public Task<User> Register(RegisterDto registerDto);  
        public Task<LoginResponseDto> Login(LoginDto loginDto);
    }
}
