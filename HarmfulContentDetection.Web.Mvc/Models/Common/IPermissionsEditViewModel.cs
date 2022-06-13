using System.Collections.Generic;
using HarmfulContentDetection.Roles.Dto;

namespace HarmfulContentDetection.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}