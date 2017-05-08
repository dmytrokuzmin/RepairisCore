using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Repairis.Web.Views
{
    public abstract class RepairisRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected RepairisRazorPage()
        {
            LocalizationSourceName = RepairisConsts.LocalizationSourceName;
        }
    }
}
