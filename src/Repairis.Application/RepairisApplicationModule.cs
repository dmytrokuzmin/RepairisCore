using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Repairis.Authorization;

namespace Repairis
{
    [DependsOn(
        typeof(RepairisCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class RepairisApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<RepairisAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RepairisApplicationModule).GetAssembly());
        }
    }
}