using EasyGiftsBackend.Domain.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.DTOs.GroupDTOs
{
    public class GroupDto
    {
        public required string Name { get; set; }
        public required UserDto Admin { get; set; }
    }
}
