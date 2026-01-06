using EasyGiftsBackend.Application.Interfaces;
using EasyGiftsBackend.Domain.DTOs;
using EasyGiftsBackend.Infrastructure.Data;
using EasyGiftsBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace EasyGiftsBackend.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthService(UserManager<IdentityUser> userManager, AppDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager; 

        }
        public async Task<User> Register(RegisterDto registerDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDto.Email.Split("@")[0],
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User registration failed: {errors}");
            }

            var user = new User
            {
                IdentityId = identityUser.Id,
                Username = identityUser.UserName,
                Email = identityUser.Email
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginDto.Email);

            if (identityUser == null)
                throw new Exception("Invalid email or password");

            var result = await _signInManager.PasswordSignInAsync(
                identityUser.UserName,
                loginDto.Password,
                true,
                false
            );

            if (!result.Succeeded)
                throw new Exception("Invalid email or password");

            return new LoginResponseDto
            {
                Id = identityUser.Id,
                Email = identityUser.Email,
                Username = identityUser.UserName
            };

        }
    }
}
