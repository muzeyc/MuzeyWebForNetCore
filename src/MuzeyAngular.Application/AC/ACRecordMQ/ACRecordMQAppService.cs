using Abp.Application.Services;
using BusinessLogic;
using CommonUtils;
using System;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACRecordMQAppService
    {
        public Dictionary<string, Dictionary<string, string>> replaceColDic;
        public Dictionary<string, Dictionary<string, string>> configReplaceColDic;

        public ACRecordMQAppService()
        {
            replaceColDic = new Dictionary<string, Dictionary<string, string>>();
            var aeCol = new Dictionary<string, string>();
            aeCol.Add("BECarType", "");
            aeCol.Add("BodySelCode", "");
            aeCol.Add("BEOnSeq", "");
            aeCol.Add("PEOnSeq", "");
            aeCol.Add("InBETime", "");
            aeCol.Add("InPETime", "");
            aeCol.Add("SKID", "");
            aeCol.Add("PANum", "");
            aeCol.Add("MCNum", "");
            aeCol.Add("FFNum", "");
            aeCol.Add("RCNum", "");
            aeCol.Add("SILNum", "");
            aeCol.Add("SIRNum", "");
            aeCol.Add("SOLNum", "");
            aeCol.Add("SORNum", "");
            aeCol.Add("FTLNum", "");
            aeCol.Add("FTRNum", "");
            aeCol.Add("SALNum", "");
            aeCol.Add("SARNum", "");
            aeCol.Add("RTLNum", "");
            aeCol.Add("RTRNum", "");

            var beCol = new Dictionary<string, string>();
            beCol.Add("AECarType", "");
            beCol.Add("QcosIp", "");
            beCol.Add("QcosJobs", "");

            replaceColDic.Add("AE", aeCol);
            replaceColDic.Add("BE", beCol);

            configReplaceColDic = new Dictionary<string, Dictionary<string, string>>();
            aeCol = new Dictionary<string, string>();
            aeCol.Add("BECarType", "");
            aeCol.Add("ConfigType", "");
            aeCol.Add("PlcToMqHeart", "");
            aeCol.Add("PlcToMqRequest", "");
            aeCol.Add("MqToPlcResult", "");
            aeCol.Add("PlcToMqReceivedFinish", "");
            aeCol.Add("PlcToMqPartNum", "");
            aeCol.Add("PlcToMqReportType", "");
            aeCol.Add("PlcToMqOrderNum", "");
            aeCol.Add("PlcToMqOrderType", "");
            aeCol.Add("PlcToMqVIN", "");
            aeCol.Add("PlcToMqCarType", "");
            aeCol.Add("PlcToMqCarFun", "");
            aeCol.Add("PlcToMqConType", "");
            aeCol.Add("PlcToMqLSH", "");
            aeCol.Add("PlcToMqFFNum", "");
            aeCol.Add("PlcToMqFTLNum", "");
            aeCol.Add("PlcToMqFTRNum", "");
            aeCol.Add("PlcToMqMCNum", "");
            aeCol.Add("PlcToMqRCNum", "");
            aeCol.Add("PlcToMqSkidNum", "");
            aeCol.Add("PlcToMqPANum", "");
            aeCol.Add("PlcToMqRTLNum", "");
            aeCol.Add("PlcToMqRTRNum", "");
            aeCol.Add("PlcToMqSALNum", "");
            aeCol.Add("PlcToMqSARNum", "");
            aeCol.Add("PlcToMqSILNum", "");
            aeCol.Add("PlcToMqSIRNum", "");
            aeCol.Add("PlcToMqSOLNum", "");
            aeCol.Add("PlcToMqSORNum", "");
            aeCol.Add("PlcToMqREV001", "");
            aeCol.Add("PlcToMqREV002", "");
            aeCol.Add("PlcToMqREV003", "");
            aeCol.Add("PlcToMqREV004", "");
            aeCol.Add("PlcToMqREV005", "");
            aeCol.Add("PlcToMqREV006", "");
            aeCol.Add("PlcToMqREV007", "");
            aeCol.Add("PlcToMqREV008", "");
            aeCol.Add("PlcToMqREV009", "");
            aeCol.Add("PlcToMqREV010", "");
            aeCol.Add("PlcToMqREV011", "");
            aeCol.Add("PlcToMqREV012", "");
            aeCol.Add("PlcToMqREV013", "");
            aeCol.Add("PlcToMqREV014", "");

            beCol = new Dictionary<string, string>();
            beCol.Add("AECarType", "");
            beCol.Add("TopCategory", "");
            beCol.Add("Reclassify", "");
            beCol.Add("PlcToMqSignal1", "");
            beCol.Add("PlcToMqSignal2", "");
            beCol.Add("PlcToMqSignal3", "");
            beCol.Add("PlcToMqValue1", "");
            beCol.Add("PlcToMqValue2", "");
            beCol.Add("PlcToMqValue3", "");
            beCol.Add("MqToPlcSignal1", "");
            beCol.Add("MqToPlcSignal2", "");
            beCol.Add("MqToPlcSignal3", "");
            beCol.Add("MqToPlcValue1", "");
            beCol.Add("MqToPlcValue2", "");
            beCol.Add("MqToPlcValue3", "");

            configReplaceColDic.Add("AE", aeCol);
            configReplaceColDic.Add("BE", beCol);
        }

        public MuzeyResModel<ACRecordMQResDto> GetDtoData(MuzeyReqModel<ACRecordMQReqDto> reqModel, bool allFlag = false)
        {
            var reportTypeDic = new Dictionary<string, string>();
            reportTypeDic.Add("S", "上线申报");
            reportTypeDic.Add("X", "下线申报");
            reportTypeDic.Add("L", "拉出申报");
            reportTypeDic.Add("H", "拉入申报");
            reportTypeDic.Add("Z", "转挂申报");
            reportTypeDic.Add("P", "计划拉出");
            reportTypeDic.Add("Q", "质量拉出");

            var typeDic = new Dictionary<string, string>();
            typeDic.Add("0", "普通申报");
            typeDic.Add("1", "关键申报");

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRecordMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_REPORT_MQDto>(filter.workShop + "※" + filter.workShop + "_AVI");
            dal.ReplaceCol(replaceColDic[filter.workShop]);
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            //var datas = dal.GetPageList(strWhere, "VIN,ReportTime", reqModel.offset, reqModel.pageSize, out totalCount);
            List<AVI_REPORT_MQDto> datas = null;
            if (allFlag)
            {
                datas = dal.GetDtoList(strWhere);
            }
            else
            {
                datas = dal.GetPageList(strWhere, "ReportTime DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            }

            resModel.totalCount = totalCount;
            foreach (var data in datas)
            {
                var rd = new ACRecordMQResDto();
                ModelUtil.Copy(data, rd);
                if(filter.workShop == "AE")
                {
                    rd.wsCarType = data.AECarType;
                }
                else
                {
                    rd.wsCarType = data.BECarType;
                }
                rd.vin = data.VIN;
                rd.orderNum = data.OrderNum;
                rd.planTypeName = "整车计划订单";
                rd.model = data.AECarType;
                if (reportTypeDic.ContainsKey(data.ReportType))
                {
                    rd.reportTypeName = reportTypeDic[data.ReportType];
                }
                rd.reportTime = data.ReportTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                rd.reportID = data.ReportID;
                rd.type = data.Type;
                if (!string.IsNullOrEmpty(rd.type))
                {
                    rd.typeName = typeDic[rd.type];
                }
                if (PhysicsPoint.code_nameDic.ContainsKey(data.ReportID))
                {
                    rd.reportName = PhysicsPoint.code_nameDic[data.ReportID];
                }
                if (PhysicsPoint.code_statusDic.ContainsKey(data.ReportID))
                {
                    rd.reportStatus = PhysicsPoint.code_statusDic[data.ReportID];
                }
               
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACRecordMQResDto> GetData(MuzeyReqModel<ACRecordMQReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRecordMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_REPORT_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            var dataModel = new ACRecordMQResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new AVI_REPORT_MQDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACRecordMQResDto> Save(MuzeyReqModel<ACRecordMQReqDto> reqModel)
        {

            var data = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACRecordMQResDto>();
            if (string.IsNullOrEmpty(data.saveData.VIN) || string.IsNullOrEmpty(data.saveData.WorkShop) || string.IsNullOrEmpty(data.saveData.ReportID) || string.IsNullOrEmpty(data.saveData.ReportType))
            {
                resModel.resStatus = "err";
                resModel.resMsg = "录入数据不完整，请检查！";
                return resModel;
            }

            //查询LineType
            var dalConfig = new MuzeyBusinessLogic<AVI_CONFIG_MQDto>(data.saveData.WorkShop + "※" + data.saveData.WorkShop + "_AVI");
            dalConfig.ReplaceCol(configReplaceColDic[data.saveData.WorkShop]);
            var configDto = dalConfig.GetDtoList(string.Format("AND Station='{0}'", data.saveData.ReportID))[0];
            data.saveData.LineType = configDto.LineType;

            var dal = new MuzeyBusinessLogic<AVI_REPORT_MQDto>(data.saveData.WorkShop + "※" + data.saveData.WorkShop + "_AVI");
            dal.ReplaceCol(replaceColDic[data.saveData.WorkShop]);
            var dalWM = new MuzeyBusinessLogic<AVI_WORKPLAN_MQDto>(data.saveData.WorkShop + "※" + data.saveData.WorkShop + "_AVI");
            dalWM.ReplaceCol(replaceColDic[data.saveData.WorkShop]);

            List<AVI_WORKPLAN_MQDto> wmDtos = null;
            if (data.saveData.WorkShop == "BE")
            {
                //如果为焊装车间LineType=0则查分线计划
                if(configDto.LineType == "0")
                {
                    var dalWF = new MuzeyBusinessLogic<AVI_WORKPLAN_DA_MQDto>(data.saveData.WorkShop + "※" + data.saveData.WorkShop + "_AVI");
                    dalWF.ChangeTableName(string.Format("AVI_WORKPLAN_{0}_MQ", configDto.Line.Replace("1","")));
                    var dtoWFs = dalWF.GetDtoList(string.Format("AND VIN='{0}' AND DownloadState='{1}'", data.saveData.VIN, 1));
                    if (!(dtoWFs.Count > 0))
                    {
                        resModel.resStatus = "err";
                        resModel.resMsg = "当前VIN码没有分线计划或下发状态未未下发无法追加过点记录";
                        return resModel;
                    }
                }
            }

            if(data.saveData.WorkShop == "AE" || configDto.LineType == "1")
            {
                //查询主线计划是否有记录
                wmDtos = dalWM.GetDtoList(string.Format("AND VIN='{0}' AND DownloadState='{1}'", data.saveData.VIN, 1));
                if (!(wmDtos.Count > 0))
                {
                    resModel.resStatus = "err";
                    resModel.resMsg = "当前VIN码没有主线计划或下发状态未未下发无法追加过点记录";
                    return resModel;
                }
            }

            //重复申报Check
            var rDto = dal.GetDtoList(string.Format("AND VIN='{0}' AND ReportID='{1}'", data.saveData.VIN, data.saveData.ReportID));
            if(rDto.Count > 0)
            {
                resModel.resStatus = "err";
                resModel.resMsg = "当前VIN码在本工位已申报,禁止重复申报";
                return resModel;
            }

            data.saveData.Type = "0";
            data.saveData.ReportState = "0";
            data.saveData.ReportSystem = 1;
            data.saveData.ReportSystemTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            data.saveData.ReportTime = DateTime.Parse(data.saveData.ReportTime.ToDateTime().ToString("yyyy-MM-dd") + " " + data.reportTime);
            ModelUtil.Copy(wmDtos[0], data.saveData);
            data.saveData.Line = configDto.Line;
            data.saveData.ID = null;
            if (string.IsNullOrEmpty(data.saveData.ID.ToStr()))
            {
                dal.InsertDto(data.saveData);
            }
            else
            {
                dal.UpdateDtoToPart(data.saveData);
            }
            return resModel;
        }

        public MuzeyResModel<ACRecordMQResDto> Delete(MuzeyReqModel<ACRecordMQReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRecordMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_REPORT_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            dal.DeleteDto(data.saveData);
            return resModel;
        }

        public MuzeyResModel<ACRecordMQResDto> GetSelectData(MuzeyReqModel<ACRecordMQReqDto> reqModel)
        {
            var data = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACRecordMQResDto>();
            var dal = new MuzeyBusinessLogic<AVI_CONFIG_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            dal.ReplaceCol(configReplaceColDic[data.workShop]);
            var rm = new ACRecordMQResDto();
            var strWhere = "";
            if (data.workShop == "AE")
            {
                strWhere += "AND TopCategory='1' AND Reclassify=1";
            }
            else
            {
                strWhere += "AND ConfigType='1'";
            }
            rm.rIdOps = dal.GetSelectList(strWhere + " Order By Station", "Station", "Station");
            if(data.workShop == "AE")
            {
                for (int i = 0; i < rm.rIdOps.Count; i++)
                {
                    rm.rIdOps[i].text = string.Format("({0})" + PhysicsPoint.code_nameDic[rm.rIdOps[i].val], rm.rIdOps[i].val);
                }

                rm.rIdOps.Insert(0,new MuzeySelectModel() { text="",val=""});
            }
            else
            {
                var sms = new List<MuzeySelectModel>();
                sms.Add(new MuzeySelectModel() { text = "", val = "" });
                foreach (var s in PhysicsPoint.stations)
                {
                    for (int i = 0; i < rm.rIdOps.Count; i++)
                    {
                        if (rm.rIdOps[i].val.Contains(s))
                        {
                            sms.Add(new MuzeySelectModel() { text = string.Format("({0})" + PhysicsPoint.code_nameDic[rm.rIdOps[i].val], rm.rIdOps[i].val), val = rm.rIdOps[i].val });
                        }
                    }
                }
                rm.rIdOps = sms;
            }
            resModel.datas.Add(rm);
            return resModel;
        }

        public MuzeyResModel<ACRecordMQResDto> GetDatas(MuzeyReqModel<ACRecordMQReqDto> reqModel)
        {
            return GetDtoData(reqModel);
        }

        public MuzeyResModel<ACRecordMQResDto> Export(MuzeyReqModel<ACRecordMQReqDto> reqModel)
        {
            var req = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACRecordMQResDto>();
            if (DateUtil.DateDiff(req.sTime.ToDateTime(), req.eTime.ToDateTime(), "Days") > 31 || string.IsNullOrEmpty(req.sTime) || string.IsNullOrEmpty(req.eTime))
            {
                resModel.CreateErr("只能导出时间段为1个月的数据！");
                return resModel;
            }
            var wb = ExcelUtil.ListToExcel<ACRecordMQResDto>(GetDtoData(reqModel, true).datas, reqModel.fileName.Split('.')[0], reqModel.cols);
            resModel.bs = new List<byte>(ExcelUtil.GetExcelBs(wb, reqModel.fileName));

            return resModel;
        }
    }

    public static class PhysicsPoint
    {
        public static Dictionary<string, string> code_nameDic = new Dictionary<string, string>();
        public static Dictionary<string, string> code_statusDic= new Dictionary<string, string>();
        public static List<string> stations = new List<string>();

        static PhysicsPoint()
        {
            code_nameDic.Add("001", "内饰一上线"); code_statusDic.Add("001", "6150");
            code_nameDic.Add("026", "内饰一下线"); code_statusDic.Add("026", "6200");
            code_nameDic.Add("027", "内饰二上线"); code_statusDic.Add("027", "6250");
            code_nameDic.Add("052", "内饰二下线"); code_statusDic.Add("052", "6300");
            code_nameDic.Add("102", "底盘一上线"); code_statusDic.Add("102", "6350");
            code_nameDic.Add("114", "底盘一下线"); code_statusDic.Add("114", "6400");
            code_nameDic.Add("116", "底盘二上线"); code_statusDic.Add("116", "6450");
            code_nameDic.Add("130", "底盘二下线"); code_statusDic.Add("130", "6500");
            code_nameDic.Add("201", "终装一线上线"); code_statusDic.Add("201", "6550");
            code_nameDic.Add("224", "终装一线下线"); code_statusDic.Add("224", "6600");
            code_nameDic.Add("225", "终装二线上线"); code_statusDic.Add("225", "6650");
            code_nameDic.Add("248", "总装下线"); code_statusDic.Add("248", "7000");
            code_nameDic.Add("501", "报交线上线"); code_statusDic.Add("501", "1000");
            code_nameDic.Add("503", "报交线下线"); code_statusDic.Add("503", "1000");
            code_nameDic.Add("601", "动力总成上线"); code_statusDic.Add("601", "1000");
            code_nameDic.Add("605", "动力总成下线"); code_statusDic.Add("605", "9000");
            code_nameDic.Add("701L", "左车门上线"); code_statusDic.Add("701L", "1000");
            code_nameDic.Add("701R", "右车门上线"); code_statusDic.Add("701R", "1000");
            code_nameDic.Add("720L", "左车门下线"); code_statusDic.Add("720L", "9000");
            code_nameDic.Add("720R", "右车门下线"); code_statusDic.Add("720R", "9000");
            code_nameDic.Add("801", "仪表上线"); code_statusDic.Add("801", "1000");
            code_nameDic.Add("820", "仪表下线"); code_statusDic.Add("820", "9000");
            code_nameDic.Add("DA1000", "前围DA上线"); code_statusDic.Add("DA1000", "");
            code_nameDic.Add("DA1080", "前围DA下线"); code_statusDic.Add("DA1080", "");
            code_nameDic.Add("DL1010", "后举门DL上线"); code_statusDic.Add("DL1010", "");
            code_nameDic.Add("DL1210", "后举门DL下线"); code_statusDic.Add("DL1210", "");
            code_nameDic.Add("ENPE31", "进涂装"); code_statusDic.Add("ENPE31", "4000");
            code_nameDic.Add("EXED31", "出电泳"); code_statusDic.Add("EXED31", "4100");
            code_nameDic.Add("ENSL31", "进空中密封线UBS"); code_statusDic.Add("ENSL31", "4200");
            code_nameDic.Add("EXSL31", "出空中密封线UBS"); code_statusDic.Add("EXSL31", "4300");
            code_nameDic.Add("ENCS31", "进分色区"); code_statusDic.Add("ENCS31", "4400");
            code_nameDic.Add("ENTC31", "进面漆"); code_statusDic.Add("ENTC31", "4500");
            code_nameDic.Add("EXFL31", "出精修"); code_statusDic.Add("EXFL31", "4600");
            code_nameDic.Add("ENRP31", "进油漆返修区"); code_statusDic.Add("ENRP31", "4700");
            code_nameDic.Add("EXTT31", "出双色车包扎线"); code_statusDic.Add("EXTT31", "4800");
            code_nameDic.Add("EXPE31", "出涂装"); code_statusDic.Add("EXPE31", "5000");
            code_nameDic.Add("FD1010", "左前门FDL上线"); code_statusDic.Add("FD1010", "");
            code_nameDic.Add("FD1330", "左前门FDL下线"); code_statusDic.Add("FD1330", "");
            code_nameDic.Add("FD1000", "右前门FDR上线"); code_statusDic.Add("FD1000", "");
            code_nameDic.Add("FD1320", "右前门FDR下线"); code_statusDic.Add("FD1320", "");
            code_nameDic.Add("FF1001", "前地板FF上线"); code_statusDic.Add("FF1001", "");
            code_nameDic.Add("FF1160", "前地板FF下线"); code_statusDic.Add("FF1160", "");
            code_nameDic.Add("FO3000", "外总拼FO上线"); code_statusDic.Add("FO3000", "1700");
            code_nameDic.Add("FO3100", "外总拼FO下线"); code_statusDic.Add("FO3100", "1750");
            code_nameDic.Add("FI1000", "内总拼FI上线"); code_statusDic.Add("FI1000", "1500");
            code_nameDic.Add("FI1100", "内总拼FI下线"); code_statusDic.Add("FI1100", "1550");
            code_nameDic.Add("HD1010", "前盖HD上线"); code_statusDic.Add("HD1010", "");
            code_nameDic.Add("HD1100", "前盖HD下线"); code_statusDic.Add("HD1100", "");
            code_nameDic.Add("MC1002", "机舱MC上线"); code_statusDic.Add("MC1002", "");
            code_nameDic.Add("MC1360", "机舱MC下线"); code_statusDic.Add("MC1360", "");
            code_nameDic.Add("MF1000", "装配线MF1上线"); code_statusDic.Add("MF1000", "2500");
            code_nameDic.Add("MF1120", "装配线MF1下线"); code_statusDic.Add("MF1120", "2600");
            code_nameDic.Add("MF3000", "调整线MF3上线"); code_statusDic.Add("MF3000", "2700");
            code_nameDic.Add("MF3120", "调整线MF3下线"); code_statusDic.Add("MF3120", "3000");
            code_nameDic.Add("MF3160", "WBS入口"); code_statusDic.Add("MF3160", "3100");
            code_nameDic.Add("MF3170", "WBS出口"); code_statusDic.Add("MF3170", "3200");
            code_nameDic.Add("MF3180", "机器人VIN校验"); code_statusDic.Add("MF3180", "3300");
            code_nameDic.Add("MF3190", "焊涂换撬点"); code_statusDic.Add("MF3190", "3400");
            code_nameDic.Add("P01", "预内饰上线"); code_statusDic.Add("P01", "6050");
            code_nameDic.Add("P09", "预内饰下线"); code_statusDic.Add("P09", "6100");
            code_nameDic.Add("PBS001", "涂总换撬"); code_statusDic.Add("PBS001", "5100");
            code_nameDic.Add("PBS002", "PBS入"); code_statusDic.Add("PBS002", "5200");
            code_nameDic.Add("PBS003", "PBS出口"); code_statusDic.Add("PBS003", "6000");
            code_nameDic.Add("PBS004", "内饰一换撬点"); code_statusDic.Add("PBS004", "6150");
            code_nameDic.Add("RC1010", "后地板RC上线"); code_statusDic.Add("RC1010", "");
            code_nameDic.Add("RC1320", "后地板RC下线"); code_statusDic.Add("RC1320", "");
            code_nameDic.Add("RD1010", "左后门RDL上线"); code_statusDic.Add("RD1010", "");
            code_nameDic.Add("RD1330", "左后门RDL下线"); code_statusDic.Add("RD1330", "");
            code_nameDic.Add("RD1000", "右后门RDR上线"); code_statusDic.Add("RD1000", "");
            code_nameDic.Add("RD1320", "右后门RDR下线"); code_statusDic.Add("RD1320", "");
            code_nameDic.Add("RF1010", "顶盖RF上线"); code_statusDic.Add("RF1010", "");
            code_nameDic.Add("RF1070", "顶盖RF下线"); code_statusDic.Add("RF1070", "");
            code_nameDic.Add("RL2000", "补焊二RL2上线"); code_statusDic.Add("RL2000", "1600");
            code_nameDic.Add("RL2100", "补焊二RL2下线"); code_statusDic.Add("RL2100", "1650");
            code_nameDic.Add("RL4000", "补焊四RL4上线"); code_statusDic.Add("RL4000", "1800");
            code_nameDic.Add("RL4100", "补焊四RL4下线"); code_statusDic.Add("RL4100", "1850");
            code_nameDic.Add("RL5000", "补焊五RL5上线"); code_statusDic.Add("RL5000", "1900");
            code_nameDic.Add("RL5090", "补焊五RL5下线"); code_statusDic.Add("RL5090", "1950");
            code_nameDic.Add("RL6000", "补焊六RL6上线"); code_statusDic.Add("RL6000", "2000");
            code_nameDic.Add("RL6100", "补焊六RL6下线"); code_statusDic.Add("RL6100", "2050");
            code_nameDic.Add("SI010L", "左侧围内板SIL上线"); code_statusDic.Add("SI010L", "");
            code_nameDic.Add("SI050L", "左侧围内板SIL下线"); code_statusDic.Add("SI050L", "");
            code_nameDic.Add("SI010R", "右侧围内板SIR上线"); code_statusDic.Add("SI010R", "");
            code_nameDic.Add("SI050R", "右侧围内板SIR下线"); code_statusDic.Add("SI050R", "");
            code_nameDic.Add("SO010L", "左侧围外板SOL上线"); code_statusDic.Add("SO010L", "");
            code_nameDic.Add("SO050L", "左侧围外板SOL下线"); code_statusDic.Add("SO050L", "");
            code_nameDic.Add("SO010R", "右侧围外板SOR上线"); code_statusDic.Add("SO010R", "");
            code_nameDic.Add("SO050R", "右侧围外板SOR下线"); code_statusDic.Add("SO050R", "");
            code_nameDic.Add("UB1020", "地板UB上线"); code_statusDic.Add("UB1020", "1100");
            code_nameDic.Add("UB1080", "地板UB下线"); code_statusDic.Add("UB1080", "1150");
            code_nameDic.Add("UR1090", "下车体补焊一UR1上线"); code_statusDic.Add("UR1090", "1200");
            code_nameDic.Add("UR1180", "下车体补焊一UR1下线"); code_statusDic.Add("UR1180", "1250");
            code_nameDic.Add("UR1190", "下车体补焊二UR2上线"); code_statusDic.Add("UR1190", "1300");
            code_nameDic.Add("UR1270", "下车体补焊二UR2下线"); code_statusDic.Add("UR1270", "1350");

            stations.Add("DA");
            stations.Add("MC");
            stations.Add("FF");
            stations.Add("RC");
            stations.Add("SI");
            stations.Add("SO");
            //stations.Add("SIL");
            //stations.Add("SIR");
            //stations.Add("SOL");
            //stations.Add("SOR");
            stations.Add("RF");
            stations.Add("HD");
            stations.Add("DL");
            stations.Add("FD");
            stations.Add("RD");
            //stations.Add("FDL");
            //stations.Add("FDR");
            //stations.Add("RDL");
            //stations.Add("RDR");
            stations.Add("UB");
            stations.Add("UR1");
            stations.Add("UR2");
            stations.Add("FI");
            stations.Add("RL2");
            stations.Add("FO");
            stations.Add("RL4");
            stations.Add("RL5");
            stations.Add("RL6");
            stations.Add("MF1");
            stations.Add("MF3");
        }
    }
}

