using BusinessLogic;

namespace MuzeyAngular.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public ApplicationInfoDto Application { get; set; }

        public UserLoginInfoDto User { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }
        public BASE_PERSONDto Person { get; set; }
    }
}
