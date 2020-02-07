using Abp.Application.Services;
using BusinessLogic;
using CommonUtils;
using System;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACStopTimeAppService
    {
        public MuzeyResModel<ACStopTimeResDto> GetDtoData(MuzeyReqModel<ACStopTimeReqDto> reqModel, bool allFlag = false)
        {
            var filter = reqModel.datas.Count > 0 ? reqModel.datas[0] : new ACStopTimeReqDto();
            MuzeyBusinessLogic<STOPTIME_INFODto> dal = null;
            string dbName = "";
            dbName = filter.workShop + "※" + filter.workShop + "_ANDON";

            dal = new MuzeyBusinessLogic<STOPTIME_INFODto>(dbName);
            var lineDic = new MuzeyBusinessLogic<BASE_LINEDto>(dbName).GetDtoDic("", "LineCode");
            var stationDic = new MuzeyBusinessLogic<BASE_STATIONDto>(dbName).GetDtoDic("", "StationCode");
            var aTypeDic = new MuzeyBusinessLogic<ALARM_ALARMTYPEDto>(dbName).GetDtoDic("", "AlarmTypeCode");
            var dTypeDic = new MuzeyBusinessLogic<ALARM_DEVICETYPEDto>(dbName).GetDtoDic("", "DeviceTypeCode");
            var sTypeDic = new MuzeyBusinessLogic<ALARM_SYSTEMDto>(dbName).GetDtoDic("", "SystemTypeCode");
            var sStatusDic = new Dictionary<string, string>();
            sStatusDic.Add("1", "停线开始");
            sStatusDic.Add("2", "停线恢复");

            var resModel = new MuzeyResModel<ACStopTimeResDto>();
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);

            List<STOPTIME_INFODto> datas = null;
            if (allFlag)
            {
                datas = dal.GetDtoList(strWhere);
            }
            else
            {
                datas = dal.GetPageList(strWhere, "StartTime DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            }
            resModel.totalCount = totalCount;
            foreach (var data in datas)
            {
                var rd = new ACStopTimeResDto();
                ModelUtil.Copy(data, rd);
                rd.sTime = data.StartTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                data.EndTime = data.EndTime == null ? DateTime.Now : data.EndTime;
                rd.eTime = data.EndTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                rd.continueTime = DateUtil.DateDiff(data.StartTime.ToDateTime(), data.EndTime.ToDateTime());

                //中文化
                if (!string.IsNullOrEmpty(data.LineCode))
                {
                    rd.lineName = lineDic[data.LineCode.ToStr()].LineFullName;
                }
                if (!string.IsNullOrEmpty(data.StationCode))
                {
                    rd.stationName = stationDic[data.StationCode.ToStr()].StationName;
                }
                if (data.StopStatus != null)
                {
                    rd.stopStatusName = sStatusDic[data.StopStatus.ToStr()];
                }
                //rd.alarmTypeName = aTypeDic[data.AlarmTypeCode.ToStr()].AlarmTypeDesc;
                //rd.deviceTypeName = dTypeDic[data.DeviceTypeCode.ToStr()].DeviceTypeName;
                //rd.alarmSysName = sTypeDic[data.SystemTypeCode.ToStr()].SystemTypeName;


                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACStopTimeResDto> GetDatas(MuzeyReqModel<ACStopTimeReqDto> reqModel)
        {
            return GetDtoData(reqModel);
        }

        public MuzeyResModel<ACStopTimeResDto> Export(MuzeyReqModel<ACStopTimeReqDto> reqModel)
        {
            var req = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACStopTimeResDto>();
            if (DateUtil.DateDiff(req.sTime.ToDateTime(), req.eTime.ToDateTime(),"Days") > 31 || string.IsNullOrEmpty(req.sTime) || string.IsNullOrEmpty(req.eTime))
            {
                resModel.CreateErr("只能导出时间段为1个月的数据！");
                return resModel;
            }
            var wb = ExcelUtil.ListToExcel<ACStopTimeResDto>(GetDtoData(reqModel, true).datas, reqModel.fileName.Split('.')[0], reqModel.cols);
            resModel.bs = new List<byte>(ExcelUtil.GetExcelBs(wb, reqModel.fileName));

            return resModel;
        }
    }
}

