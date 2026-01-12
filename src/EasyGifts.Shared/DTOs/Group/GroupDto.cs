using EasyGifts.Shared.DTOs.User;

namespace EasyGifts.Shared.DTOs.Group;

public class GroupDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required UserDto Admin { get; set; }
}
