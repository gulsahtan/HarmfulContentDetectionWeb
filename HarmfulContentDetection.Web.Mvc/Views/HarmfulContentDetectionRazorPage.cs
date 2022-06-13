using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace HarmfulContentDetection.Web.Views
{
    public abstract class HarmfulContentDetectionRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected HarmfulContentDetectionRazorPage()
        {
            LocalizationSourceName = HarmfulContentDetectionConsts.LocalizationSourceName;
        }
    }
}
