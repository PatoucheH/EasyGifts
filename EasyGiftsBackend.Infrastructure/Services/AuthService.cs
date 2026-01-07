using EasyGiftsBackend.Application.Interfaces;
using EasyGiftsBackend.Domain.DTOs;
using EasyGiftsBackend.Domain.Entities;
using EasyGiftsBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EasyGiftsBackend.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(
            UserManager<IdentityUser> userManager, 
            AppDbContext context, 
            IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;

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
        private async Task<string> GenerateJwt(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new Exception("Invalid email or password");

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                throw new Exception("Invalid email or password");

            return new LoginResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Token = await GenerateJwt(user)
            };
        }
    }
}
