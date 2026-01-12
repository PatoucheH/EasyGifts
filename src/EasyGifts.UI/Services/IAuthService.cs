using EasyGifts.Shared.DTOs.Auth;

namespace EasyGifts.UI.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task LogoutAsync();
    Task<LoginResponseDto?> GetCurrentUserAsync();
    Task<bool> IsAuthenticatedAsync();
}
