using System;
using System.Collections.Generic;
using System.Text;
using EasyGiftsBackend.Domain.DTOs.GiftDTOs;
using EasyGiftsBackend.Domain.Entities;

namespace EasyGiftsBackend.Application.Interfaces
{
    public interface IGiftsService
    {
        public Task<GiftDto> CreateGift(GiftDtoCreate giftDto);
        public Task<string> DeleteGift(Guid giftId);
        public Task<GiftDto> UpdateGift(GiftDto giftDto);
        public Task<GiftDto> GetGiftByGiftId(Guid giftId);
        public Task<List<GiftDto>> GetGiftsByUserId(Guid UserId);
        public Task<List<GiftDto>> GetGiftsCurrentUser();
    }
}
