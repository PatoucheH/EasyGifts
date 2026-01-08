using EasyGiftsBackend.Application.Interfaces;
using EasyGiftsBackend.Domain.DTOs.GroupDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyGiftsBackend.Api.Controllers
{
    [ApiController]
    [Route("api/groups")]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(CreateGroupRequest groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName.GroupName))
                return BadRequest("Group name is required");

            try
            {
                var group = await _groupService.CreateGroup(groupName.GroupName);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{groupId}/invite")]
        public async Task<IActionResult> InviteUser(Guid groupId, [FromBody] InviteUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required");

            try
            {
                var result = await _groupService.AddUserToGroup(groupId, request.Email);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(Guid groupId)
        {
            try
            {
                var result = await _groupService.DeleteGroup(groupId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
