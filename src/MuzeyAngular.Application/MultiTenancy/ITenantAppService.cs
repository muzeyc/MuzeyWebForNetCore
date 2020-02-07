using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MuzeyAngular.MultiTenancy.Dto;

namespace MuzeyAngular.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

