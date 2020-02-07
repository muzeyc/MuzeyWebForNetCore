using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using MuzeyAngular.Sessions.Dto;
using MuzeyServer;

namespace MuzeyAngular.Sessions
{
    public class SessionAppService : MuzeyAngularAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
                var bds = new List<BaseDataReqDto>();
                bds.Add(new BaseDataReqDto() { user= output.User.Id.ToString()});
                var resData = new BaseDataAppService().GetLogin(new MuzeyReqModel<BaseDataReqDto>()
                {
                    datas = bds
                });
                if(resData.datas.Count > 0)
                {
                    output.Person = resData.datas[0];
                }
                
            }

            return output;
        }
    }
}
