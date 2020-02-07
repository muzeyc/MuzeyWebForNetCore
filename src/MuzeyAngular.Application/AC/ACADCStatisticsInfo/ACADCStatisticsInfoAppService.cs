using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACADCStatisticsInfoAppService
    {
        public MuzeyResModel<ACADCStatisticsInfoResDto> GetDatas(MuzeyReqModel<ACADCStatisticsInfoReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACADCStatisticsInfoResDto>();
            var dalLine = new MuzeyBusinessLogic<BASE_LINEDto>("SE※SE_ANDON");
            var dicLine = dalLine.GetDtoDic("", "LineCode");
            var dal = new MuzeyBusinessLogic<ADC_TIME_STATISTICSDto>("SE※SE_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "StartTime DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACADCStatisticsInfoResDto();
                ModelUtil.Copy(data, rd);
                rd.AdcResultName = data.AdcResult == 1 ? "成功" : "失败";
                rd.LineCodeName = dicLine[data.LineCode].LineFullName;
                resModel.datas.Add(rd);
            }
            return resModel;
        }
    }
}

