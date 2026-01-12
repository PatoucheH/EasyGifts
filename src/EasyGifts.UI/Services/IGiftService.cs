using EasyGifts.Shared.DTOs.Gift;

namespace EasyGifts.UI.Services;

public interface IGiftService
{
    Task<List<GiftDto>> GetMyGiftsAsync();
    Task<List<GiftDto>> GetGiftsByUserIdAsync(Guid userId);
    Task<GiftDto?> GetGiftByIdAsync(Guid giftId);
    Task<GiftDto?> CreateGiftAsync(GiftCreateDto gift);
    Task<bool> UpdateGiftAsync(GiftDto gift);
    Task<bool> DeleteGiftAsync(Guid giftId);
}
