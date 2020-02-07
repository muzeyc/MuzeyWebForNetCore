using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MuzeyAngular.Roles.Dto;
using MuzeyAngular.Users.Dto;

namespace MuzeyAngular.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
