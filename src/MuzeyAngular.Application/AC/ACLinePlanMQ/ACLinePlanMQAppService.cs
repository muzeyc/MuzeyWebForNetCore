using BusinessLogic;
using CommonUtils;
using System;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACLinePlanMQAppService
    {
        public MuzeyResModel<ACLinePlanMQResDto> GetDtoData(MuzeyReqModel<ACLinePlanMQReqDto> reqModel, bool allFlag = false)
        {

            var filter = reqModel.datas[0];
            var tableList = new List<string>();
            if (string.IsNullOrEmpty(filter.lineCode))
            {
                tableList.Add("AVI_WORKPLAN_DA_MQ");
                tableList.Add("AVI_WORKPLAN_DL_MQ");
                tableList.Add("AVI_WORKPLAN_FDL_MQ");
                tableList.Add("AVI_WORKPLAN_FDR_MQ");
                tableList.Add("AVI_WORKPLAN_FF_MQ");
                tableList.Add("AVI_WORKPLAN_HD_MQ");
                tableList.Add("AVI_WORKPLAN_MC_MQ");
                tableList.Add("AVI_WORKPLAN_RC_MQ");
                tableList.Add("AVI_WORKPLAN_RDL_MQ");
                tableList.Add("AVI_WORKPLAN_RDR_MQ");
                tableList.Add("AVI_WORKPLAN_RF_MQ");
                tableList.Add("AVI_WORKPLAN_SIL_MQ");
                tableList.Add("AVI_WORKPLAN_SIR_MQ");
                tableList.Add("AVI_WORKPLAN_SOL_MQ");
                tableList.Add("AVI_WORKPLAN_SOR_MQ");
            }
            else
            {
                tableList.Add(string.Format("AVI_WORKPLAN_{0}_MQ",filter.lineCode));
            }

            var resModel = new MuzeyResModel<ACLinePlanMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_WORKPLAN_DA_MQDto>(filter.workShop + "※" + filter.workShop + "_AVI");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            if (!string.IsNullOrEmpty(filter.workDone))
            {
                strWhere += string.Format(" AND (workDone='{0}' {1})", filter.workDone, filter.workDone == "0" ? "or workDone IS NULL" : "");
            }
            //var datas = dal.GetSamePageList(strWhere, "VIN,DateTime", reqModel.offset, reqModel.pageSize, out totalCount, tableList);
            List<AVI_WORKPLAN_DA_MQDto> datas = null;
            if (allFlag)
            {
                datas = dal.GetDtoList(strWhere);
            }
            else
            {
                datas = dal.GetSamePageList(strWhere, "VIN,DateTime", reqModel.offset, reqModel.pageSize, out totalCount, tableList);
            }

            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACLinePlanMQResDto();
                ModelUtil.Copy(data, rd);
                rd.WorkdoneName = data.Workdone == 1 ? "完工" : "未完工";
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACLinePlanMQResDto> Workdone(MuzeyReqModel<ACLinePlanMQReqDto> reqModel)
        {
            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACLinePlanMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_WORKPLAN_DA_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            dal.ChangeTableName(string.Format("AVI_WORKPLAN_{0}_MQ", data.lineCode));
            data.dto.Workdone = 1;
            data.dto.WorkdoneTime = DateTime.Now;
            dal.UpdateDtoToPart(data.dto);
            return resModel;
        }

        public MuzeyResModel<ACLinePlanMQResDto> GetDatas(MuzeyReqModel<ACLinePlanMQReqDto> reqModel)
        {
            return GetDtoData(reqModel);
        }

        public MuzeyResModel<ACLinePlanMQResDto> UploadExcel(MuzeyReqModel<ACLinePlanMQReqDto> reqModel)
        {
            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACLinePlanMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_WORKPLAN_DA_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            dal.ChangeTableName(string.Format("AVI_WORKPLAN_{0}_MQ", data.lineCode));
            var dt = ExcelUtil.ExcelToDataTable(data.file, "Sheet1", true);
            return resModel;
        }

        public MuzeyResModel<ACLinePlanMQResDto> Export(MuzeyReqModel<ACLinePlanMQReqDto> reqModel)
        {
            var req = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACLinePlanMQResDto>();
            //if (CommonUtils.DateUtil.DateDiff(req.sTime.ToDateTime(), req.eTime.ToDateTime(), "Days") > 31 || string.IsNullOrEmpty(req.sTime) || string.IsNullOrEmpty(req.eTime))
            //{
            //    resModel.CreateErr("只能导出时间段为1个月的数据！");
            //    return resModel;
            //}
            var wb = ExcelUtil.ListToExcel<ACLinePlanMQResDto>(GetDtoData(reqModel, true).datas, reqModel.fileName.Split('.')[0], reqModel.cols);
            resModel.bs = new List<byte>(ExcelUtil.GetExcelBs(wb, reqModel.fileName));

            return resModel;
        }
    }
}

