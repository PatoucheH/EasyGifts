namespace EasyGifts.Shared.DTOs.Gift;

public class GiftCreateDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? Url { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPurchased { get; set; } = true;
    public Guid UserId { get; set; }
}
