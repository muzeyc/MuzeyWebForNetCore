using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MuzeyAngular.Configuration;

namespace MuzeyAngular.Web.Host.Startup
{
    [DependsOn(
       typeof(MuzeyAngularWebCoreModule))]
    public class MuzeyAngularWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MuzeyAngularWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MuzeyAngularWebHostModule).GetAssembly());
        }
    }
}
