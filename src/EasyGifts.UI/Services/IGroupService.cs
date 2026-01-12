using EasyGifts.Shared.DTOs.Group;
using EasyGifts.Shared.DTOs.User;

namespace EasyGifts.UI.Services;

public interface IGroupService
{
    Task<GroupDto?> GetMyGroupAsync();
    Task<GroupDto?> GetGroupByIdAsync(Guid groupId);
    Task<List<UserDto>> GetGroupMembersAsync(Guid groupId);
    Task<GroupDto?> CreateGroupAsync(string groupName);
    Task<bool> InviteUserAsync(Guid groupId, string email);
    Task<bool> RemoveUserAsync(Guid groupId, Guid userId);
    Task<bool> DeleteGroupAsync(Guid groupId);
}
