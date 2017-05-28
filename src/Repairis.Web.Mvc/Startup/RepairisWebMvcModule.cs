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

            // Email
            if (bool.Parse(_appConfiguration["EmailSettings:IsEnabled"]))
            {
                Configuration.Get<EmailSettings>().IsEnabled = bool.Parse(_appConfiguration["EmailSettings:IsEnabled"]);
                Configuration.Get<EmailSettings>().ApiKey = _appConfiguration["EmailSettings:ApiKey"];
                Configuration.Get<EmailSettings>().ApiBaseUri = _appConfiguration["EmailSettings:ApiBaseUri"];
                Configuration.Get<EmailSettings>().RequestUri = _appConfiguration["EmailSettings:RequestUri"];
                Configuration.Get<EmailSettings>().From = _appConfiguration["EmailSettings:From"];
            }

            // Sms
            if (bool.Parse(_appConfiguration["SmsSettings:IsEnabled"]))
            {
                Configuration.Get<SmsSettings>().IsEnabled = bool.Parse(_appConfiguration["SmsSettings:IsEnabled"]);
                Configuration.Get<SmsSettings>().Sid = _appConfiguration["SmsSettings:Sid"];
                Configuration.Get<SmsSettings>().Token = _appConfiguration["SmsSettings:Token"];
                Configuration.Get<SmsSettings>().BaseUri = _appConfiguration["SmsSettings:BaseUri"];
                Configuration.Get<SmsSettings>().RequestUri = _appConfiguration["SmsSettings:RequestUri"];
                Configuration.Get<SmsSettings>().From = _appConfiguration["SmsSettings:From"];
            }

            // Company
            Configuration.Get<CompanySettings>().CompanyName = _appConfiguration["Company:Name"];
            Configuration.Get<CompanySettings>().CompanyStreet = _appConfiguration["Company:Street"];
            Configuration.Get<CompanySettings>().CompanyCity = _appConfiguration["Company:City"];
            Configuration.Get<CompanySettings>().CompanyState = _appConfiguration["Company:State"];
            Configuration.Get<CompanySettings>().CompanyContactNumber = _appConfiguration["Company:ContactNumber"];
            Configuration.Get<CompanySettings>().CompanyEmail = _appConfiguration["Company:Email"];
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RepairisWebMvcModule).GetAssembly());
        }
    }
}