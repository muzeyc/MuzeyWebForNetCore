using Abp.Application.Services;
using BusinessLogic;
using CommonUtils;
using System;
using System.Collections.Generic;
using System.IO;

namespace MuzeyServer
{
    public class ACWarnAppService
    {
        public ACWarnAppService()
        {
        }

        public MuzeyResModel<ACWarnResDto> GetDtoData(MuzeyReqModel<ACWarnReqDto> reqModel, bool allFlag = false)
        {
            var filter = reqModel.datas.Count > 0 ? reqModel.datas[0] : new ACWarnReqDto();
            MuzeyBusinessLogic<ALARM_INFODto> dal = null;
            string dbName = "";
            dbName = filter.workShop + "※" + filter.workShop + "_ANDON";

            dal = new MuzeyBusinessLogic<ALARM_INFODto>(dbName);
            var lineDic = new MuzeyBusinessLogic<BASE_LINEDto>(dbName).GetDtoDic("", "LineCode");
            var stationDic = new MuzeyBusinessLogic<BASE_STATIONDto>(dbName).GetDtoDic("", "StationCode");
            var aTypeDic = new MuzeyBusinessLogic<ALARM_ALARMTYPEDto>(dbName).GetDtoDic("", "AlarmTypeCode");
            var dTypeDic = new MuzeyBusinessLogic<ALARM_DEVICETYPEDto>(dbName).GetDtoDic("", "DeviceTypeCode");
            var sTypeDic = new MuzeyBusinessLogic<ALARM_SYSTEMDto>(dbName).GetDtoDic("", "SystemTypeCode");
            var aStatuDic = new Dictionary<string, string>();
            aStatuDic.Add("1", "报警触发");
            aStatuDic.Add("2", "报警复位");
            aStatuDic.Add("3", "报警后造成停线");

            var resModel = new MuzeyResModel<ACWarnResDto>();
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            List<ALARM_INFODto> datas = null;
            if (!string.IsNullOrEmpty(filter.alarmTypeCode))
            {
                strWhere += string.Format("AND alarmTypeCode{0}", filter.alarmTypeCode.Contains(",") ? string.Format(" in({0})", filter.alarmTypeCode) : string.Format("='{0}'", filter.alarmTypeCode));
            }
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
                var rd = new ACWarnResDto();
                ModelUtil.Copy(data, rd);
                rd.sTime = data.StartTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                rd.eTime = data.EndTime == null ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : data.EndTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                rd.continueT = DateUtil.DateDiff(data.StartTime.ToDateTime(), data.EndTime== null ? DateTime.Now : data.EndTime.ToDateTime());

                //中文化
                if (data.LineCode != null)
                {
                    rd.lineName = lineDic[data.LineCode.ToStr()].LineFullName;
                }
                if (data.StationCode != null)
                {
                    rd.stationName = stationDic[data.StationCode.ToStr()].StationName;
                }
                if (data.AlarmTypeCode != null)
                {
                    rd.alarmTypeName = aTypeDic[data.AlarmTypeCode.ToStr()].AlarmTypeDesc;
                }
                if (data.DeviceTypeCode != null)
                {
                    rd.deviceTypeName = dTypeDic[data.DeviceTypeCode.ToStr()].DeviceTypeName;
                }
                if (data.SystemTypeCode != null)
                {
                    rd.alarmSysName = sTypeDic[data.SystemTypeCode.ToStr()].SystemTypeName;
                }
                if (data.AlarmStatus != null)
                {
                    rd.alarmStatuName = aStatuDic[data.AlarmStatus.ToStr()];
                }

                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACWarnResDto> GetDatas(MuzeyReqModel<ACWarnReqDto> reqModel)
        {
            return GetDtoData(reqModel);
        }

        public MuzeyResModel<ACWarnResDto> Export(MuzeyReqModel<ACWarnReqDto> reqModel)
        {
            var req = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACWarnResDto>();
            if (DateUtil.DateDiff(req.sTime.ToDateTime(), req.eTime.ToDateTime(), "Days") > 31 || string.IsNullOrEmpty(req.sTime) || string.IsNullOrEmpty(req.eTime))
            {
                resModel.CreateErr("只能导出时间段为1个月的数据！");
                return resModel;
            }
            var wb = ExcelUtil.ListToExcel<ACWarnResDto>(GetDtoData(reqModel, true).datas, reqModel.fileName.Split('.')[0], reqModel.cols);
            resModel.bs = new List<byte>(ExcelUtil.GetExcelBs(wb, reqModel.fileName));

            return resModel;
        }
    }
}

