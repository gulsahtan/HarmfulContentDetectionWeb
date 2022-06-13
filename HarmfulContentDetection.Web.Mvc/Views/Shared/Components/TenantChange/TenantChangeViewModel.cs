using Abp.AutoMapper;
using HarmfulContentDetection.Sessions.Dto;

namespace HarmfulContentDetection.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}
