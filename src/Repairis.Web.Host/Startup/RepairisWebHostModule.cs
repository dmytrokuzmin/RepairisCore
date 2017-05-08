using System.Reflection;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Repairis.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Repairis.Web.Host.Startup
{
    [DependsOn(
       typeof(RepairisWebCoreModule))]
    public class RepairisWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public RepairisWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RepairisWebHostModule).GetAssembly());
        }
    }
}
