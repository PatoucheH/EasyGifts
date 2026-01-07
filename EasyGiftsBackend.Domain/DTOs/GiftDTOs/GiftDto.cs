using EasyGiftsBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.DTOs.GiftDTOs
{
    public class GiftDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Url { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPurchased { get; set; } = true;
        public Guid UserId { get; set; }
    }
}
