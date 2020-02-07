using System.Threading.Tasks;
using Abp.Application.Services;
using MuzeyAngular.Authorization.Accounts.Dto;

namespace MuzeyAngular.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
