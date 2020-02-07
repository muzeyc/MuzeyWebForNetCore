using System.Threading.Tasks;
using Abp.Application.Services;
using MuzeyAngular.Sessions.Dto;

namespace MuzeyAngular.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
