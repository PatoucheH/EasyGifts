using System;
using System.Collections.Generic;
using System.Text;
using EasyGiftsBackend.Domain.DTOs.GroupDTOs;

namespace EasyGiftsBackend.Application.Interfaces
{
    public interface IGroupService
    {
        public Task<GroupDto> CreateGroup(string groupName);
        public Task<string> DeleteGroup(Guid groupId);
        public Task<string> AddUserToGroup(Guid groupId, string email);
        public Task<string> RemoveUserFromGroup(Guid groupId, Guid userId);

    }
}
