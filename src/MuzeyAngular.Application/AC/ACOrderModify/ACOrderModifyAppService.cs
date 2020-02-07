using BusinessLogic;
using CommonUtils;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACOrderModifyAppService
    {
        public Dictionary<string, Dictionary<string, string>> replaceColDic;

        public ACOrderModifyAppService()
        {
            replaceColDic = new Dictionary<string, Dictionary<string, string>>();
            var aeCol = new Dictionary<string, string>();

            var beCol = new Dictionary<string, string>();
            beCol.Add("PreOrderNum", "");
            beCol.Add("PreVIN", "");

            replaceColDic.Add("AE", aeCol);
            replaceColDic.Add("BE", beCol);
        }

        public MuzeyResModel<ACOrderModifyResDto> GetDatas(MuzeyReqModel<ACOrderModifyReqDto> reqModel)
        {
            var typeWSDic = new Dictionary<string, Dictionary<string, string>>();
            var typeAEDic = new Dictionary<string, string>();
            typeAEDic.Add("1","车辆拉出");
            typeAEDic.Add("2", "车辆拉入");
            typeWSDic.Add("AE", typeAEDic);

            var typeBEDic = new Dictionary<string, string>();
            typeBEDic.Add("1", "订单撤回");
            typeBEDic.Add("2", "订单替换");
            typeBEDic.Add("3", "订单报废");
            typeBEDic.Add("4", "车辆拉出");
            typeWSDic.Add("BE", typeBEDic);

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACOrderModifyResDto>();
            var dal = new MuzeyBusinessLogic<AVI_ORDER_MODIFY_MQDto>(filter.workShop + "※" + filter.workShop + "_AVI");
            dal.ReplaceCol(replaceColDic[filter.workShop]);
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "DateTime DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACOrderModifyResDto();
                ModelUtil.Copy(data, rd);
                rd.Type = typeWSDic[filter.workShop][data.Type];
                rd.ModifyState = data.ModifyState == "1" ? "接收成功" : "接收驳回";
                rd.DownloadPlcStr = data.DownloadPlc == 0 ? "未下发" : "已下发";
                resModel.datas.Add(rd);
            }
            return resModel;
        }
    }
}

