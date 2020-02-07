using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACADCSingleAppService
    {
        public MuzeyResModel<ACADCSingleResDto> GetDatas(MuzeyReqModel<ACADCSingleReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACADCSingleResDto>();
            var dal = new MuzeyBusinessLogic<ADC_SINGLE_OUTPUTDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACADCSingleResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }
    }
}

