using EasyGiftsBackend.Application.Interfaces;
using EasyGiftsBackend.Domain.DTOs.GiftDTOs;
using EasyGiftsBackend.Domain.Entities;
using EasyGiftsBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace EasyGiftsBackend.Infrastructure.Services
{
    public class GiftsService : IGiftsService
    {
        public AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GiftsService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity!.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("UserId claim not found");

            var appUser = _context.AppUsers.FirstOrDefault(u => u.IdentityId == userIdClaim.Value);

            return appUser!.Id;
        }

        public async Task<GiftDto> CreateGift(GiftDtoCreate giftDto)
        {
            var userId = GetCurrentUserId();
            if (giftDto == null)
                throw new ArgumentNullException(nameof(giftDto));
            if (giftDto.Name == null)
                throw new ArgumentException("Gift name cannot be null");
            if (giftDto.Price < 0)
                throw new ArgumentException("Gift price cannot be negative");
            if (string.IsNullOrWhiteSpace(giftDto.Name))
                throw new ArgumentException("Gift name cannot be null or empty");
            var gift = new Gift
            {
                Name = giftDto.Name,
                Description = giftDto.Description,
                Price = giftDto.Price,
                Url = giftDto.Url,
                ImageUrl = giftDto.ImageUrl,
                IsPurchased = giftDto.IsPurchased,
                UserId = userId
            };
            var result = _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
            if (result == null)
            {
                throw new Exception("Failed to create gift");
            }
            return new GiftDto
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                Price = gift.Price,
                Url = gift.Url,
                ImageUrl = gift.ImageUrl,
                IsPurchased = gift.IsPurchased,
                UserId = userId
            };
        }

        public async Task<string> DeleteGift(Guid giftId)
        {
            if (giftId == Guid.Empty)
                throw new ArgumentException("Gift ID cannot be empty");
            var gift = await _context.Gifts.FindAsync(giftId);
            if (gift == null)
                throw new Exception("Gift not found");
            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();
            return "Gift deleted successfully";
        }
        public async Task<GiftDto> UpdateGift(GiftDto giftDto)
        {
            var gift = _context.Gifts.Find(giftDto.Id);
            if (gift == null)
                throw new ArgumentNullException(nameof(gift));
            if (giftDto.Price < 0)
                throw new ArgumentException("Gift price cannot be negative");
            if (string.IsNullOrWhiteSpace(giftDto.Name))
                throw new ArgumentException("Gift name cannot be null or empty");
            gift.Name = giftDto.Name;
            gift.Description = giftDto.Description;
            gift.Price = giftDto.Price;
            gift.Url = giftDto.Url;
            gift.ImageUrl = giftDto.ImageUrl;
            gift.IsPurchased = giftDto.IsPurchased;
            _context.Gifts.Update(gift);
            await _context.SaveChangesAsync();
            return new GiftDto
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                Price = gift.Price,
                Url = gift.Url,
                ImageUrl = gift.ImageUrl,
                IsPurchased = gift.IsPurchased,
                UserId = gift.UserId
            };
        }
        public async Task<GiftDto> GetGiftByGiftId(Guid giftId)
        {
            if (giftId == Guid.Empty)
                throw new ArgumentException("Gift ID cannot be empty");
            var gift = await _context.Gifts.FindAsync(giftId);
            if (gift == null)
                throw new Exception("Gift not found");
            return new GiftDto
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                Price = gift.Price,
                Url = gift.Url,
                ImageUrl = gift.ImageUrl,
                IsPurchased = gift.IsPurchased,
                UserId = gift.UserId
            };
        }
        public async Task<List<GiftDto>> GetGiftsByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            var gifts = await _context.Gifts
                .Where(g => g.UserId == userId)
                .Select(g => new GiftDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    Price = g.Price,
                    Url = g.Url,
                    ImageUrl = g.ImageUrl,
                    IsPurchased = g.IsPurchased,
                    UserId = g.UserId
                })
                .ToListAsync();

            return gifts;
        }

        public async Task<List<GiftDto>> GetGiftsCurrentUser()
        {
            var userId = GetCurrentUserId();

            var gifts = await _context.Gifts
                .Where(g => g.UserId == userId)
                .Select(g => new GiftDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    Price = g.Price,
                    Url = g.Url,
                    ImageUrl = g.ImageUrl,
                    IsPurchased = g.IsPurchased,
                    UserId = g.UserId
                })
                .ToListAsync();

            return gifts;
        }
    }
}
