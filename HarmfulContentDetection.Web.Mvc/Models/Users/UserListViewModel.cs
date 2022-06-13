using System.Collections.Generic;
using HarmfulContentDetection.Roles.Dto;

namespace HarmfulContentDetection.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
