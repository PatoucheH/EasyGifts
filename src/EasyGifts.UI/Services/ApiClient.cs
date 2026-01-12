using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using EasyGifts.Shared.DTOs.Auth;
using EasyGifts.Shared.DTOs.Gift;
using EasyGifts.Shared.DTOs.Group;
using EasyGifts.Shared.DTOs.User;

namespace EasyGifts.UI.Services;

public class ApiClient : IAuthService, IGiftService, IGroupService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";
    private const string UserKey = "currentUser";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    private async Task SetAuthHeaderAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(TokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    #region Auth

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
        if (!response.IsSuccessStatusCode) return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>(JsonOptions);
        if (result?.Token != null)
        {
            await _localStorage.SetItemAsync(TokenKey, result.Token);
            await _localStorage.SetItemAsync(UserKey, result);
        }
        return result;
    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(UserKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<LoginResponseDto?> GetCurrentUserAsync()
    {
        await SetAuthHeaderAsync();
        try
        {
            var response = await _httpClient.GetAsync("api/auth/me");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<LoginResponseDto>(JsonOptions);
        }
        catch
        {
            return await _localStorage.GetItemAsync<LoginResponseDto>(UserKey);
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(TokenKey);
        return !string.IsNullOrEmpty(token);
    }

    #endregion

    #region Gifts

    public async Task<List<GiftDto>> GetMyGiftsAsync()
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.GetAsync("api/gift/myGifts");
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<GiftDto>>(JsonOptions) ?? [];
    }

    public async Task<List<GiftDto>> GetGiftsByUserIdAsync(Guid userId)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.GetAsync($"api/gift/getByUserId?userId={userId}");
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<GiftDto>>(JsonOptions) ?? [];
    }

    public async Task<GiftDto?> GetGiftByIdAsync(Guid giftId)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.GetAsync($"api/gift/getByGiftId?giftId={giftId}");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<GiftDto>(JsonOptions);
    }

    public async Task<GiftDto?> CreateGiftAsync(GiftCreateDto gift)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/gift/create", gift);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<GiftDto>(JsonOptions);
    }

    public async Task<bool> UpdateGiftAsync(GiftDto gift)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/gift/update", gift);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteGiftAsync(Guid giftId)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/gift/delete", new { Id = giftId });
        return response.IsSuccessStatusCode;
    }

    #endregion

    #region Groups

    public async Task<GroupDto?> GetMyGroupAsync()
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.GetAsync("api/groups/me");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<GroupDto>(JsonOptions);
    }

    public async Task<GroupDto?> GetGroupByIdAsync(Guid groupId)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.GetAsync($"api/groups/{groupId}");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<GroupDto>(JsonOptions);
    }

    public async Task<List<UserDto>> GetGroupMembersAsync(Guid groupId)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.GetAsync($"api/groups/{groupId}/members");
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<UserDto>>(JsonOptions) ?? [];
    }

    public async Task<GroupDto?> CreateGroupAsync(string groupName)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/groups", new CreateGroupRequest { GroupName = groupName });
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<GroupDto>(JsonOptions);
    }

    public async Task<bool> InviteUserAsync(Guid groupId, string email)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync($"api/groups/{groupId}/invite", new InviteUserRequest { Email = email });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveUserAsync(Guid groupId, Guid userId)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/groups/{groupId}/members/{userId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteGroupAsync(Guid groupId)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/groups/{groupId}");
        return response.IsSuccessStatusCode;
    }

    #endregion
}
