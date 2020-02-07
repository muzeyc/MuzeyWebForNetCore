using Abp.Application.Services.Dto;

namespace MuzeyAngular.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

