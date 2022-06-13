using System.Collections.Generic;
using HarmfulContentDetection.Roles.Dto;

namespace HarmfulContentDetection.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
