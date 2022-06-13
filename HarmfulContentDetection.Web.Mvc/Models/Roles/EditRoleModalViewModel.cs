using Abp.AutoMapper;
using HarmfulContentDetection.Roles.Dto;
using HarmfulContentDetection.Web.Models.Common;

namespace HarmfulContentDetection.Web.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class EditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool HasPermission(FlatPermissionDto permission)
        {
            return GrantedPermissionNames.Contains(permission.Name);
        }
    }
}
