using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACMaterielCallAppService
    {
        public MuzeyResModel<ACMaterielCallResDto> GetDatas(MuzeyReqModel<ACMaterielCallReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMaterielCallResDto>();
            var dal = new MuzeyBusinessLogic<AVI_MATERIELCALLDto>(filter.workShop + "※" + filter.workShop + "_AVI");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACMaterielCallResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }
    }
}

