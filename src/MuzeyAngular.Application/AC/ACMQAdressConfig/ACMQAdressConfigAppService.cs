using BusinessLogic;
using CommonUtils;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACMQAdressConfigAppService
    {

        public Dictionary<string, Dictionary<string, string>> replaceColDic;

        public ACMQAdressConfigAppService()
        {
            replaceColDic = new Dictionary<string, Dictionary<string, string>>();

            var aeCol = new Dictionary<string, string>();
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

            var beCol = new Dictionary<string, string>();
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

            replaceColDic.Add("AE", aeCol);
            replaceColDic.Add("BE", beCol);
        }

        public MuzeyResModel<ACMQAdressConfigResDto> GetDatas(MuzeyReqModel<ACMQAdressConfigReqDto> reqModel)
        {
            var filter = reqModel.datas[0];

            var dalLine = new MuzeyBusinessLogic<BASE_LINEDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var lineDic = dalLine.GetDtoDic("","LineCode");

            var resModel = new MuzeyResModel<ACMQAdressConfigResDto>();
            var dal = new MuzeyBusinessLogic<AVI_CONFIG_MQDto>(filter.workShop + "※" + filter.workShop + "_AVI");
            if (replaceColDic.ContainsKey(filter.workShop))
            {
                dal.ReplaceCol(replaceColDic[filter.workShop]);
            }
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            var recDic = new Dictionary<string, string>();
            recDic.Add("1", "请求信号");
            recDic.Add("2", "校验信号");
            recDic.Add("3", "完成信号");
            foreach (var data in datas)
            {
                var rd = new ACMQAdressConfigResDto();
                ModelUtil.Copy(data, rd);
                rd.LineType = data.LineType == "1" ? "主线" : "分线";
                rd.KeyPointName = data.KeyPoint == 1 ? "关键" : "普通";
                if (filter.workShop == "AE")
                {
                    rd.TopCategory = data.TopCategory == "1" ? "过点申报" : "快速请求车身信号";
                    if (recDic.ContainsKey(data.Reclassify))
                    {
                        rd.Reclassify = recDic[data.Reclassify];
                    }
                    
                    if (lineDic.ContainsKey(data.Line))
                    {
                        rd.Line = lineDic[data.Line].LineFullName;
                    }
                }
                else
                {
                    rd.ConfigType = data.ConfigType == "1" ? "过点申报" : "心跳";
                }
                
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACMQAdressConfigResDto> GetData(MuzeyReqModel<ACMQAdressConfigReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMQAdressConfigResDto>();
            var dal = new MuzeyBusinessLogic<AVI_CONFIG_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            if (replaceColDic.ContainsKey(data.workShop))
            {
                dal.ReplaceCol(replaceColDic[data.workShop]);
            }
            var dataModel = new ACMQAdressConfigResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new AVI_CONFIG_MQDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACMQAdressConfigResDto> Save(MuzeyReqModel<ACMQAdressConfigReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMQAdressConfigResDto>();
            var dal = new MuzeyBusinessLogic<AVI_CONFIG_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
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

        public MuzeyResModel<ACMQAdressConfigResDto> Delete(MuzeyReqModel<ACMQAdressConfigReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMQAdressConfigResDto>();
            var dal = new MuzeyBusinessLogic<AVI_CONFIG_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

