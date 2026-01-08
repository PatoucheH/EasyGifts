using EasyGiftsBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Domain.DTOs.GroupDTOs
{
    public class GroupDto
    {
        public required string Name { get; set; }
        public required User Admin { get; set; }
    }
}
