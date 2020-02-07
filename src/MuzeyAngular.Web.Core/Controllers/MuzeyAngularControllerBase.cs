using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace MuzeyAngular.Controllers
{
    public abstract class MuzeyAngularControllerBase: AbpController
    {
        protected MuzeyAngularControllerBase()
        {
            LocalizationSourceName = MuzeyAngularConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
