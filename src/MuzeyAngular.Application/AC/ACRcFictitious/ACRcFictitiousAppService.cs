using BusinessLogic;
using CommonUtils;
using System;
using System.Collections.Generic;
using System.Data;

namespace MuzeyServer
{
    public class ACRcFictitiousAppService
    {
        public MuzeyResModel<ACRcFictitiousResDto> GetDatas(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {
            var stateDic = new Dictionary<string,string>();

            var filter = reqModel.datas[0];
            
            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var dal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dal.ChangeTableName(filter.rcType);
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetDtoList(strWhere);
            resModel.totalCount = datas.Count;
            var roadDic = new Dictionary<string, List<RC_CacheDto>>();
            foreach(var data in datas)
            {
                if (roadDic.ContainsKey(data.Road))
                {
                    roadDic[data.Road].Add(data);
                }
                else
                {
                    roadDic.Add(data.Road,new List<RC_CacheDto>() { data });
                    stateDic.Add(data.Road, data.PlaceState);
                }
            }

            foreach(var kv in roadDic)
            {
                var ls = new ACRcFictitiousResDto();
                var t = typeof(ACRcFictitiousResDto);
                foreach(var d in kv.Value)
                {
                    var p = t.GetProperty("col" + d.Place);
                    p.SetValue(ls,d.VIN);
                    p = t.GetProperty("state" + d.Place);
                    p.SetValue(ls, d.State);
                    p = t.GetProperty("seq" + d.Place);
                    p.SetValue(ls, d.Seq.ToStr());
                }

                ls.InRoadState = stateDic[kv.Key][0] == '1' ? "lock" : "lock_open";
                ls.OutRoadState = stateDic[kv.Key][1] == '1' ? "lock" : "lock_open";
                ls.DjRoadState = stateDic[kv.Key][2] == '1' ? "1" : "0";
                resModel.datas.Add(ls);
            }

            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> GetData(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var dal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dal.ChangeTableName(data.rcType);
            var dataModel = new ACRcFictitiousResDto();
            dataModel.saveData = dal.GetDtoList(string.Format("and Area='{0}' and Road='{1}' and Place='{2}'"
                , data.saveData.Area, data.saveData.Road.PadLeft(2, '0')
                , (data.saveData.Place.ToInt() + 1).ToString().PadLeft(2,'0')
                ))[0];
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> Save(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var dal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dal.ChangeTableName(data.rcType);
            data.saveData.CacheCode = null;
            data.saveData.PlaceState = null;
            dal.UpdateDtoToPart(data.saveData, string.Format("AND AREA='{0}' AND ROAD='{1}' AND PLACE='{2}'", data.saveData.Area, data.saveData.Road, data.saveData.Place));

            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> Delete(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var dal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dal.ChangeTableName(data.rcType);
            dal.DeleteDto(data.saveData);
            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> CleanSeq(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var dal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dal.ChangeTableName(data.rcType);
            dal.UpdateDtoToPart(new RC_CacheDto() { Seq = 0 },string.Format("AND Area='{0}'", data.area));

            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> GetFData(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var workShop = data.area == "PBS" ? "AE" : "BE";
            var dbName = string.Format("◎{0}◎", workShop + "※" + workShop + "_AVI");
            DataTable dt = SqlHelp.Query(string.Format(dbName + "select FamilyCode,FeatureCode from AVI_WORKPLAN_MQ where vin='{0}' and ScrapState='0'", data.VIN)).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = SqlHelp.Query(string.Format(dbName + "select FamilyCode,FeatureCode from AVI_WORKPLAN_MQ where vin='{0}' and ScrapState='0'", data.VIN)).Tables[0].Rows[0];
                resModel.datas.Add(new ACRcFictitiousResDto() { FamilyCode = dr["FamilyCode"].ToStr(), FeatureCode = dr["FeatureCode"].ToStr() });
            }
            else
            {
                resModel.resStatus = "err";
                resModel.resMsg = "计划中无对应VIN数据！";
            }

            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> Preinstall(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {
            var charNamDic = new Dictionary<string,int>();
            charNamDic.Add("5",2);
            charNamDic.Add("6", 2);
            charNamDic.Add("B", 0);
            charNamDic.Add("C", 0);
            charNamDic.Add("D", 1);
            charNamDic.Add("E", 1);

            var stateDic = new Dictionary<string, char>();
            stateDic.Add("5", '1');
            stateDic.Add("6", '0');
            stateDic.Add("B", '1');
            stateDic.Add("C", '0');
            stateDic.Add("D", '1');
            stateDic.Add("E", '0');

            var data = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var dal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            var dalCace = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dalCace.ChangeTableName(data.rcType);
            reqModel.datas[0].savePreinstallData.OpTime = DateTime.Now.ToString();
            var opState = reqModel.datas[0].savePreinstallData.State;
            if (opState == "6" || opState == "5"
                || opState == "A" || opState == "B"
                || opState == "C" || opState == "D"
                || opState == "E")
            {
                if (string.IsNullOrEmpty(reqModel.datas[0].savePreinstallData.Road))
                {
                    resModel.CreateErr("道号不能为空！");
                    return resModel;
                }

                if(opState != "A")
                {
                    char[] stateCs = dalCace.GetDtoList(string.Format("AND Area='{0}' AND Road='{1}'", data.savePreinstallData.Area, data.savePreinstallData.Road))[0].PlaceState.ToCharArray();
                    stateCs[charNamDic[opState]] = stateDic[opState];

                    dalCace.UpdateDtoToPart(new RC_CacheDto() { PlaceState = new string(stateCs) }
                    , string.Format("AND Area='{0}' AND Road='{1}'", data.savePreinstallData.Area, data.savePreinstallData.Road));
                }
            }

            dal.InsertDto(reqModel.datas[0].savePreinstallData);
            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> Invented(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {
            var data = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var dalCaceInvented = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dalCaceInvented.ChangeTableName("RC_CacheInvented");
            var dtos = dalCaceInvented.GetDtoList("");
            var mRch = new MuzeyRCHelp();
            for(int i = 0; i < 100; i++)
            {
                //模拟排产
                var mnpcRes = mRch.OutGetRCRoad(data.area, "RC_CacheInvented");
                if (mnpcRes.state)
                {
                    var caceDto = dalCaceInvented.GetDtoList(string.Format("AND Area='{0}' AND Road like '%{1}' AND PLACE='01'", data.area, mnpcRes.roadNum.ToStr()))[0];
                    resModel.datas.Add(new ACRcFictitiousResDto() { Road=mnpcRes.roadNum.ToStr(), VIN = caceDto.VIN, OutType = mnpcRes.isRev ? "返回" : "出库" });
                    //模拟出车
                    RCCommonInvented.RCUpdateRoadOut(data.area, mnpcRes.roadNum.ToStr(), caceDto.VIN);
                }
                else if(mnpcRes.msg.Equals("缓存区无车"))
                {
                    break;
                }
                else
                {
                    resModel.resStatus = "err";
                    resModel.resMsg = mnpcRes.msg;
                    break;
                }
            }

            //还原队列
            dalCaceInvented.UpdateDtoListToAll(dtos);
            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> InRoadPc(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {
            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var data = reqModel.datas[0];
            var dalCaceInvented = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dalCaceInvented.ChangeTableName("RC_CacheInvented");
            var mRch = new MuzeyRCHelp();
            var mnpcRes = mRch.InGetRCRoad(data.area, data.VIN, "RC_CacheInvented");
            if (mnpcRes.state)
            {
                resModel.datas.Add(new ACRcFictitiousResDto()
                {
                    Road = mnpcRes.roadNum.ToStr(),
                    VIN = data.VIN,
                    OutType = "入库"
                });

                //模拟入车
                RCCommonInvented.RCUpdateRoadIn(data.area, mnpcRes.roadNum.ToStr(), data.VIN);
            }
            else
            {
                resModel.CreateErr(mnpcRes.msg);
            }

            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> CopyFictitious(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {
            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            //拷贝真实队列至模拟队列
            var dalCace = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            var dalCaceInvented = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dalCaceInvented.ChangeTableName("RC_CacheInvented");
            var caceDtos = dalCace.GetDtoList("");
            dalCaceInvented.UpdateDtoListToAll(caceDtos);
            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> OutCarPlan(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {
            var data = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();
            var dalCaceInvented = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dalCaceInvented.ChangeTableName(data.rcType);
            var mRch = new MuzeyRCHelp();

            //模拟排产
            var mnpcRes = mRch.OutGetRCRoad(data.area, "RC_CacheInvented");
            if (mnpcRes.state)
            {
                var caceDto = dalCaceInvented.GetDtoList(string.Format("AND Area='{0}' AND Road like '%{1}' AND PLACE='01'", data.area, mnpcRes.roadNum.ToStr()))[0];
                resModel.datas.Add(new ACRcFictitiousResDto() { Road = mnpcRes.roadNum.ToStr(), VIN = caceDto.VIN, OutType = mnpcRes.isRev ? "返回" : "出库" });
                //模拟出车
                //RCCommonInvented.RCUpdateRoadOut(data.area, mnpcRes.roadNum.ToStr(), caceDto.VIN);
            }
            else if (mnpcRes.msg.Equals("缓存区无车"))
            {
            }
            else
            {
                resModel.resStatus = "err";
                resModel.resMsg = mnpcRes.msg;
            }

            return resModel;
        }

        public MuzeyResModel<ACRcFictitiousResDto> OutCarUpdateDL(MuzeyReqModel<ACRcFictitiousReqDto> reqModel)
        {
            var data = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACRcFictitiousResDto>();

            RCCommonInvented.RCUpdateRoadOut(data.area, data.saveData.Road, data.saveData.VIN);
            return resModel;
        }
    }
}

