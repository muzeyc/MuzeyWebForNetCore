using BusinessLogic;
using CommonUtils;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACWorkPlanMQAppService
    {
        public MuzeyResModel<ACWorkPlanMQResDto> GetDtoData(MuzeyReqModel<ACWorkPlanMQReqDto> reqModel, bool allFlag = false)
        {
            var replaceColDic = new Dictionary<string, Dictionary<string, string>>();
            var aeCol = new Dictionary<string, string>();
            aeCol.Add("BECarType", "");
            aeCol.Add("BodySelCode", "");
            aeCol.Add("BEOnSeq", "");
            aeCol.Add("PEOnSeq", "");
            aeCol.Add("InBETime", "");
            aeCol.Add("InPETime", "");

            var beCol = new Dictionary<string, string>();
            beCol.Add("AECarType", "");
            beCol.Add("QcosIp", "");
            beCol.Add("QcosJobs", "");

            replaceColDic.Add("AE", aeCol);
            replaceColDic.Add("BE", beCol);

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWorkPlanMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_WORKPLAN_MQDto>(filter.workShop + "※" + filter.workShop + "_AVI");
            dal.ReplaceCol(replaceColDic[filter.workShop]);
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            if (!string.IsNullOrEmpty(filter.workDone))
            {
                strWhere += string.Format(" AND (workDone='{0}' {1})", filter.workDone, filter.workDone == "0" ? "or workDone IS NULL" : "");
            }
            //var datas = dal.GetPageList(strWhere, "VIN,DateTime", reqModel.offset, reqModel.pageSize, out totalCount);
            List<AVI_WORKPLAN_MQDto> datas = null;
            if (allFlag)
            {
                datas = dal.GetDtoList(strWhere);
            }
            else
            {
                // 排序
                datas = dal.GetPageList(strWhere, filter.workShop == "AE" ? "DateTime" : "BESubOnSeq" + "DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            }
            resModel.totalCount = totalCount;
            foreach (var data in datas)
            {
                var rd = new ACWorkPlanMQResDto();
                ModelUtil.Copy(data, rd);
                rd.WorkdoneName = data.Workdone == 1 ? "完工" : "未完工";
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACWorkPlanMQResDto> Workdone(MuzeyReqModel<ACWorkPlanMQReqDto> reqModel)
        {
            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWorkPlanMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_WORKPLAN_MQDto>(data.workShop + "※" + data.workShop + "_AVI");

            data.dto.Workdone = 1;
            dal.UpdateDtoToPart(data.dto);
            return resModel;
        }

        public MuzeyResModel<ACWorkPlanMQResDto> GetDatas(MuzeyReqModel<ACWorkPlanMQReqDto> reqModel)
        {
            return GetDtoData(reqModel);
        }

        public MuzeyResModel<ACWorkPlanMQResDto> UploadExcel(MuzeyReqModel<ACWorkPlanMQReqDto> reqModel)
        {
            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWorkPlanMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_WORKPLAN_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            var dt = ExcelUtil.ExcelToDataTable(data.file, "Sheet1", true);
            return resModel;
        }

        public MuzeyResModel<ACWorkPlanMQResDto> Export(MuzeyReqModel<ACWorkPlanMQReqDto> reqModel)
        {
            var req = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACWorkPlanMQResDto>();
            //if (CommonUtils.DateUtil.DateDiff(req.sTime.ToDateTime(), req.eTime.ToDateTime(), "Days") > 31 || string.IsNullOrEmpty(req.sTime) || string.IsNullOrEmpty(req.eTime))
            //{
            //    resModel.CreateErr("只能导出时间段为1个月的数据！");
            //    return resModel;
            //}
            var wb = ExcelUtil.ListToExcel<ACWorkPlanMQResDto>(GetDtoData(reqModel, true).datas, reqModel.fileName.Split('.')[0], reqModel.cols);
            resModel.bs = new List<byte>(ExcelUtil.GetExcelBs(wb, reqModel.fileName));

            return resModel;
        }
    }
}

