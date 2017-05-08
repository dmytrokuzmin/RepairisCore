using System.Reflection;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Repairis.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Repairis.Web.Startup
{
    [DependsOn(typeof(RepairisWebCoreModule))]
    public class RepairisWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public RepairisWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<RepairisNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RepairisWebMvcModule).GetAssembly());
        }
    }
}