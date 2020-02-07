using System.Threading.Tasks;
using MuzeyAngular.Configuration.Dto;

namespace MuzeyAngular.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
