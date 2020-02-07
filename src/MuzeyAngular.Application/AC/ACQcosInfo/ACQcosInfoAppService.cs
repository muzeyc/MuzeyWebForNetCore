using BusinessLogic;
using CommonUtils;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACQcosInfoAppService
    {
        public MuzeyResModel<ACQcosInfoResDto> GetDatas(MuzeyReqModel<ACQcosInfoReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var dalLine = new MuzeyBusinessLogic<BASE_LINEDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var lineDic = dalLine.GetDtoDic("", "LineName");
            var stateDic = new Dictionary<string, string>();
            stateDic.Add("1","OK");
            stateDic.Add("2", "NOK");
            stateDic.Add("3", "故障");

            var resModel = new MuzeyResModel<ACQcosInfoResDto>();
            var dal = new MuzeyBusinessLogic<ANDON_QCOS_INFODto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "VIN,QcosTime", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACQcosInfoResDto();
                ModelUtil.Copy(data, rd);
                if (lineDic.ContainsKey(data.Line))
                {
                    rd.Line = lineDic[data.Line].LineMESName;
                }
                if (lineDic.ContainsKey(data.QcosStatus))
                {
                    rd.QcosStatus = stateDic[data.QcosStatus];
                }
                rd.ReportMesStatus = data.ReportMesStatus == "1" ? "已上报" : "未上报";
                resModel.datas.Add(rd);
            }
            return resModel;
        }
    }
}

