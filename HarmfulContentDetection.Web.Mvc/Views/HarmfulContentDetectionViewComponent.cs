using Abp.AspNetCore.Mvc.ViewComponents;

namespace HarmfulContentDetection.Web.Views
{
    public abstract class HarmfulContentDetectionViewComponent : AbpViewComponent
    {
        protected HarmfulContentDetectionViewComponent()
        {
            LocalizationSourceName = HarmfulContentDetectionConsts.LocalizationSourceName;
        }
    }
}
