using Abp.Application.Services;

namespace MuzeyServer
{
    public interface IMuzeyAppService : IApplicationService
    {
        MuzeyResModel<object> Req(MuzeyReqModel<object> reqModel);
        string FileUpload(MuzeyReqModel<MuzeyUploadReqModel> reqModel);
        MuzeyMenuModel GetMenuData(string user);
    }
}
