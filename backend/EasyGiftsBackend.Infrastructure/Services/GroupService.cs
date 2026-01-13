using EasyGiftsBackend.Application.Interfaces;
using EasyGiftsBackend.Domain.DTOs.GroupDTOs;
using EasyGiftsBackend.Domain.DTOs.UserDTOs;
using EasyGiftsBackend.Domain.Entities;
using EasyGiftsBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace EasyGiftsBackend.Infrastructure.Services
{
    public class GroupService : IGroupService 
    {
        private IEmailService _emailService;
        private AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GroupService (AppDbContext context, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        private Guid GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var user = httpContext?.User;

            if (user == null || !user.Identity!.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("UserId claim not found");

            var appUser = _context.AppUsers
                .FirstOrDefault(u => u.IdentityId == userIdClaim.Value);

            if (appUser != null)
            {
                Console.WriteLine($"AppUser.Id: {appUser.Id}");
                Console.WriteLine($"AppUser.IdentityId: {appUser.IdentityId}");
            }

            if (appUser == null)
                throw new UnauthorizedAccessException("Application user not found");

            return appUser.Id;
        }

        private static UserDto ToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }


        public async Task<GroupDto> CreateGroup(string groupName)
        {
            if(string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Group name cannot be null or empty");
            var userId = GetCurrentUserId();
            User user = await _context.AppUsers.FindAsync(userId) ?? throw new Exception("user not found");

            var group = new Group 
            {                
                Name = groupName,
                AdminId = userId,
                Admin = user
            };
            var groupUser = new GroupUser
            {
                Group = group,
                User = user,
                UserId = userId
            };

            group.GroupUsers.Add(groupUser);
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Admin = ToUserDto(user),
            };

        }
        public async Task<string> DeleteGroup(Guid groupId)
        {
            var userId = GetCurrentUserId();

            var group = await _context.Groups.FindAsync(groupId)
                ?? throw new Exception("Group not found");

            if (group.AdminId != userId)
                throw new UnauthorizedAccessException("Only admin can delete the group");

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return "Group deleted successfully";
        }
        public async Task<string> AddUserToGroup(Guid groupId, string email)
        {
            var currentUserId = GetCurrentUserId();

            var group = await _context.Groups.FindAsync(groupId)
                ?? throw new Exception("Group not found");

            if (group.AdminId != currentUserId)
                throw new UnauthorizedAccessException("Only admin can add users");
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                var alreadyInGroup = await _context.GroupUsers
                    .AnyAsync(gu => gu.GroupId == groupId && gu.UserId == user.Id);

                if (alreadyInGroup)
                    return "User already in group";

                _context.GroupUsers.Add(new GroupUser
                {
                    GroupId = groupId,
                    UserId = user.Id
                });

                await _context.SaveChangesAsync();

                await _emailService.SendAsync(
                    user.Email,
                    "Ajout à un groupe",
                    $@"
                <h2>Bonjour {user.Username}</h2>
                <p>Vous avez été ajouté au groupe <b>{group.Name}</b>.</p>
            "
                );

                return "User added and notified";
            }

            var token = Guid.NewGuid().ToString("N");

            var invitation = new GroupInvitation
            {
                GroupId = groupId,
                Email = email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                Accepted = false
            };

            _context.GroupInvitations.Add(invitation);
            await _context.SaveChangesAsync();

            var invitationLink =
                $"https://localhost:5001/register?token={token}";

            await _emailService.SendAsync(
                email,
                "Invitation à rejoindre EasyGifts",
                $@"
            <h2>Invitation</h2>
            <p>Vous avez été invité à rejoindre le groupe <b>{group.Name}</b>.</p>
            <p>
                <a href='{invitationLink}'>Créer un compte</a>
            </p>
        "
            );

            return "Invitation email sent";
        }

        public async Task<string> RemoveUserFromGroup(Guid groupId, Guid userId)
        {
            var currentUserId = GetCurrentUserId();

            var group = await _context.Groups.FindAsync(groupId)
                ?? throw new Exception("Group not found");

            if (group.AdminId != currentUserId)
                throw new UnauthorizedAccessException("Only admin can remove users");

            if (group.AdminId == userId)
                throw new Exception("Admin cannot be removed from the group");

            var groupUser = await _context.GroupUsers
                .FirstOrDefaultAsync(gu => gu.GroupId == groupId && gu.UserId == userId)
                ?? throw new Exception("User is not in the group");

            _context.GroupUsers.Remove(groupUser);
            await _context.SaveChangesAsync();

            return "User removed from group successfully";
        }

        public async Task<GroupDto> GetGroupById(Guid groupId)
        {
            var group = await _context.Groups
                .Include(g => g.Admin)
                .Include(g => g.GroupUsers)
                    .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(g => g.Id == groupId)
                ?? throw new Exception("Group not found");
            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Admin = ToUserDto(group.Admin)
            };
        }

        public async Task<GroupDto?> GetGroupCurrentUser()
        {
            var currentUserId = GetCurrentUserId();

            var groupUser = await _context.GroupUsers
                .Include(gu => gu.Group)
                    .ThenInclude(g => g.Admin)
                .FirstOrDefaultAsync(gu => gu.UserId == currentUserId);

            if (groupUser == null)
                return null;

            return new GroupDto
            {
                Id = groupUser.Group.Id,
                Name = groupUser.Group.Name,
                Admin = ToUserDto(groupUser.Group.Admin)
            };
        }

        public async Task<List<GroupDto>> GetGroupsCurrentUser()
        {
            var currentUserId = GetCurrentUserId();

            var groups = await _context.GroupUsers
                .Where(gu => gu.UserId == currentUserId)
                .Include(gu => gu.Group)
                    .ThenInclude(g => g.Admin)
                .Select(gu => new GroupDto
                {
                    Id = gu.Group.Id,
                    Name = gu.Group.Name,
                    Admin = ToUserDto(gu.Group.Admin)
                })
                .ToListAsync();

            return groups;
        }


        public async Task<List<UserDto>> GetMembersOfGroup(Guid groupId)
        {
            var currentUserId = GetCurrentUserId();

            var group = await _context.Groups
                .Include(g => g.GroupUsers)
                .FirstOrDefaultAsync(g => g.Id == groupId)
                ?? throw new Exception("Group not found");

            var isAdmin = group.AdminId == currentUserId;

            var isMember = group.GroupUsers
                .Any(gu => gu.UserId == currentUserId);

            if (!isAdmin && !isMember)
                throw new UnauthorizedAccessException(
                    "You are not allowed to view members of this group");

            var members = await _context.GroupUsers
                .Where(gu => gu.GroupId == groupId)
                .Include(gu => gu.User)
                .Select(gu => ToUserDto(gu.User))
                .ToListAsync();

            return members;
        }
    }
}
