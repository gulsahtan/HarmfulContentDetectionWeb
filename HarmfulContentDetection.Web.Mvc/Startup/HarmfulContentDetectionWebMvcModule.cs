using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using HarmfulContentDetection.Configuration;

namespace HarmfulContentDetection.Web.Startup
{
    [DependsOn(typeof(HarmfulContentDetectionWebCoreModule))]
    public class HarmfulContentDetectionWebMvcModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public HarmfulContentDetectionWebMvcModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<HarmfulContentDetectionNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HarmfulContentDetectionWebMvcModule).GetAssembly());
        }
    }
}
