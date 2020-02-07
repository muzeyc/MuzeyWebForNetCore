using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MuzeyAngular.Authorization;

namespace MuzeyAngular
{
    [DependsOn(
        typeof(MuzeyAngularCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class MuzeyAngularApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<MuzeyAngularAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(MuzeyAngularApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
