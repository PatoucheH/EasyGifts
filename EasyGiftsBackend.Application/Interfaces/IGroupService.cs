using System;
using System.Collections.Generic;
using System.Text;
using EasyGiftsBackend.Domain.DTOs.GroupDTOs;
using EasyGiftsBackend.Domain.DTOs.UserDTOs;

namespace EasyGiftsBackend.Application.Interfaces
{
    public interface IGroupService
    {
        public Task<GroupDto> CreateGroup(string groupName);
        public Task<string> DeleteGroup(Guid groupId);
        public Task<string> AddUserToGroup(Guid groupId, string email);
        public Task<string> RemoveUserFromGroup(Guid groupId, Guid userId);
        public Task<GroupDto> GetGroupById(Guid groupId);
        public Task<GroupDto> GetGroupCurrentUser();
        public Task<List<UserDto>> GetMembersOfGroup(Guid groupId);
    }
}
