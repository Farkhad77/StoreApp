using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.DTOs.UserDtos
{
    public record class UserAssignRoleDto
    {
        public string UserId { get; set; }
        public string RoleName { get; set; } // məsələn: "Moderator"
    }
}
