using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using MuzeyAngular.Configuration.Dto;

namespace MuzeyAngular.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : MuzeyAngularAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
