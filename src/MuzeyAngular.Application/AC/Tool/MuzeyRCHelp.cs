using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class MuzeyRCHelp
    {
        /// <summary>
        /// 根据VIN获取道号（入口）
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="vin">VIN</param>
        /// <returns></returns>
        public RCRoadResModel InGetRCRoad(string area, string vin, string tableName = "RC_Cache")
        {
            var res = new RCRoadResModel();
            var ruleCoreObj = new RuleCore();

            //PBS快速道
            if (area == "PBS")
            {
                //写方向1:入库区 2:快速道
                var subRes = ruleCoreObj.ISUP(new RC_RuleDto() { Road = "04" }, "PBS", vin, "", tableName);
                if (subRes.state)
                {
                    return subRes;
                }
            }

            //指定车道查询
            var planRes = ruleCoreObj.IPLAN(new RC_RuleDto(), area, vin, "", tableName);
            if (planRes.state)
            {
                return planRes;
            }

            //冻结车进冻结车道
            var idcRes = ruleCoreObj.IDC(new RC_RuleDto(), area, vin, "", tableName);
            if (idcRes.state)
            {
                return idcRes;
            }

            var dalCace = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dalCace.ChangeTableName(tableName);
            //获取所有进道规则
            var ruleDal = new MuzeyBusinessLogic<RC_RuleDto>("ABP_Base");
            var ruleDtos = ruleDal.GetDtoList(string.Format("AND Area='{0}' AND InOutType='1' AND IsEnable <> '0' order by seq",area));
            var tr = typeof(RuleCore);
            //var ruleCoreObj = Activator.CreateInstance(tr);
            //核心流程
            foreach (var ruleDto in ruleDtos)
            {
                //判断该车进道是否锁定如果锁定直接GG
                if (InRoadLock(area, ruleDto.Road,tableName))
                {
                    res.state = false;
                    res.msg = string.Format("规则->{0}->车道->{1}->该车进道已锁定", ruleDto.RuleDesign, ruleDto.Road);
                    continue;
                }

                if (ruleDto.IsEnable == "2")
                {
                    var scriptArr = ruleDto.RuleScript.Split('#');
                    var m = tr.GetMethod(scriptArr[0]);
                    res = (RCRoadResModel)m.Invoke(ruleCoreObj, new object[] { ruleDto, area, vin, scriptArr[1], tableName });
                    if (res.state)
                    {
                        return res;
                    }
                }
            }

            //通用流程
            //按照车辆均衡程度获取数据
            DataTable dt = SqlHelp.Query(string.Format("◎ABP_Base◎select Road,count(*) as num from {1} where Area='{0}' AND (vin = '' or vin is null ) AND IsSup = '0' AND IsRev = '0' GROUP BY Road ORDER BY num Desc", area,tableName)).Tables[0];
            foreach(DataRow dr in dt.Rows)
            {
                if(dr["num"].ToInt() == 0)
                {
                    continue;
                }

                //判断该车道是否锁定如果锁定直接GG
                if (InRoadLock(area, dr["Road"].ToStr(), tableName))
                {
                    res.state = false;
                    res.msg = string.Format("车道->{0}->该车道已锁定", dr["Road"].ToStr());
                    continue;
                }

                //只需满足一个即可如果没有规则则直接进
                bool haveRule = false;
                foreach (var ruleDto in ruleDtos)
                {
                    if (ruleDto.IsEnable == "1")
                    {
                        haveRule = true;
                        var scriptArr = ruleDto.RuleScript.Split('#');
                        var m = tr.GetMethod(scriptArr[0]);
                        res = (RCRoadResModel)m.Invoke(ruleCoreObj, new object[] { ruleDto, area, vin, scriptArr[1], tableName });
                        if (res.state)
                        {
                            return res;
                        }
                    }
                }

                if (!haveRule)
                {
                    res.state = true;
                    res.roadNum = short.Parse(dr["Road"].ToStr());
                    return res;
                }
            }

            return res;
        }

        /// <summary>
        /// 获取道号（出口）
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        public RCRoadResModel OutGetRCRoad(string area, string tableName= "RC_Cache")
        {
            var res = new RCRoadResModel();
            //判断是否有可出车
            var caceDal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            caceDal.ChangeTableName(tableName);
            var caceDtos = caceDal.GetDtoList(string.Format("AND Area='{0}' AND VIN <> '' AND VIN is not null",area) + " AND SUBSTRING(PlaceState,2,1) <> '1' AND NOT (SUBSTRING(PlaceState,3,1) = '1' AND STATE = '1')");
            if(caceDtos.Count == 0)
            {
                res.msg = "缓存区无车";
                return res;
            }

            //获取所有出道规则
            var ruleDal = new MuzeyBusinessLogic<RC_RuleDto>("ABP_Base");
            var ruleDtos = ruleDal.GetDtoList(string.Format("AND Area='{0}' AND InOutType='2' AND IsEnable <> '0' order by seq", area));
            var tr = typeof(RuleCore);
            var ruleCoreObj = Activator.CreateInstance(tr);
            //核心流程
            foreach (var ruleDto in ruleDtos)
            {
                if (ruleDto.IsEnable == "2")
                {
                    var scriptArr = ruleDto.RuleScript.Split('#');
                    var m = tr.GetMethod(scriptArr[0]);
                    res = (RCRoadResModel)m.Invoke(ruleCoreObj, new object[] { ruleDto, area, scriptArr[1], tableName });
                    if (res.state)
                    {
                        return res;
                    }
                }
            }

            //通用流程
            //将所有符合条件的车列出
            //查询快速道是否有车
            string whereStr = " AND SUBSTRING(PlaceState,2,1) <> '1' AND NOT (SUBSTRING(PlaceState,3,1) = '1' AND STATE = '1')";
            var cacheDal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            cacheDal.ChangeTableName(tableName);
            var cacheDtos = cacheDal.GetDtoList(string.Format("AND Area='{0}' AND Place='01'", area) + whereStr + "Order By Road");
            foreach (var cacheDto in cacheDtos)
            {
                //规则判定标志
                var ruleFlag = true;
                //所有规则判定情况
                int checkOKNum = 0;
                //按照规则优先级判断
                foreach (var ruleDto in ruleDtos)
                {
                    if (ruleDto.IsEnable == "1")
                    {
                        var scriptArr = ruleDto.RuleScript.Split('#');
                        var m = tr.GetMethod(scriptArr[0]);
                        res = (RCRoadResModel)m.Invoke(ruleCoreObj, new object[] { ruleDto, area, scriptArr[1], tableName });
                        //需要全部匹配
                        if (!res.state)
                        {
                            //判断该规则是否可破坏如不可破坏直接GG
                            if (ruleDto.IsDestroy == "0")
                            {
                                ruleFlag = false;
                                break;
                            }
                        }
                        else
                        {
                            checkOKNum++;
                        }
                    }
                }

                //如果全部匹配则OK 或至少有一个匹配
                if (ruleFlag && (checkOKNum > 0))
                {
                    res.state = true;
                    res.roadNum = short.Parse(cacheDto.Road);
                    return res;
                }
            }

            return res;
        }

        public static bool RoadIsFull(string area,string road,string tableName= "RC_Cache")
        {
            var DbName = "◎ABP_Base◎";
            string sqlStr = string.Format(DbName + "select count(*) as num from {2} where Area='{0}' AND Road='{1}' AND (vin = '' or vin is null )", area, road, tableName);
            return SqlHelp.Query(sqlStr).Tables[0].Rows[0][0].ToInt() == 0;
        }

        public static bool InRoadLock(string area, string road, string tableName = "RC_Cache")
        {
            var dalCace = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dalCace.ChangeTableName(tableName);
            var djDtos = dalCace.GetDtoList(string.Format("AND Area = '{0}' AND Road = '{1}' AND PlaceState like '1%'", area,road));
            return djDtos.Count > 0;
        }

        public static bool OutRoadLock(string area, string road, string tableName = "RC_Cache")
        {
            var dalCace = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            var djDtos = dalCace.GetDtoList(string.Format("AND Area = '{0}' AND Road = '{1}' AND SUBSTRING(PlaceState,2,1) = '1'", area, road));
            return djDtos.Count > 0;
        }

        public RCScriptModel RcScriptPrase(string scriptStr,string type)
        {
            var res = new RCScriptModel();
            string[] ms;
            switch (type)
            {
                case "IEQ":
                    if(scriptStr.Split("&&").Length > 1)
                    {
                        ms = scriptStr.Split("&&");
                        AddScriptModel(res, ms[0], false);
                        AddScriptModel(res, ms[1], false);
                    }
                    else if(scriptStr.Split("||").Length > 1)
                    {
                        ms = scriptStr.Split("||");
                        AddScriptModel(res, ms[0], true);
                        AddScriptModel(res, ms[1], true);
                    }
                    else
                    {
                        AddScriptModel(res, scriptStr, false);
                    }
                    break;
            }   

            return res;
        }

        //FamilyCode=ABA  FeatureCode=ABAA
        public void AddScriptModel(RCScriptModel m, string scriptStr, bool isDestroy)
        {
            string[] ms;
            string eq;
            if(scriptStr.Split("=").Length > 1)
            {
                ms = scriptStr.Split("=");
                eq = "0";
            }
            else
            {
                ms = scriptStr.Split("!=");
                eq = "1";
            }

            eq += isDestroy ? "1" : "0";
            if (ms[0]== "FamilyCode")
            {
                m.FamilyCodeCheckDic.Add(ms[1], eq);
            }
            else
            {
                m.FeatureCodeCheckDic.Add(ms[1], eq);
            }
        }
    }

    public class RuleCore
    {
        public string whereStr = " AND SUBSTRING(PlaceState,2,1) <> '1' AND NOT (SUBSTRING(PlaceState,3,1) = '1' AND STATE = '1')";
        public RuleCore()
        {
            
        }

        //进道预排快速道规则
        public RCRoadResModel ISUP(RC_RuleDto dto, string area, string vin, string script, string tableName = "RC_Cache")
        {
            var res = new RCRoadResModel();
            var opDal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            //判断VIN是否预快速
            var dDtos = opDal.GetDtoList(string.Format("AND Area='{0}' AND VIN='{1}' AND State='9'", area, vin));
            if (dDtos.Count > 0)
            {
                res.roadNum = short.Parse(dto.Road);
                res.isSub = true;
                res.state = true;
                return res;
            }
            return res;
        }

        //进道指定车道
        public RCRoadResModel IPLAN(RC_RuleDto dto, string area, string vin, string script, string tableName = "RC_Cache")
        {
            var res = new RCRoadResModel();
            var opDal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            //判断VIN是否预快速
            var dDtos = opDal.GetDtoList(string.Format("AND Area='{0}' AND VIN='{1}' AND State='A'", area, vin));
            if (dDtos.Count > 0)
            {
                res.roadNum = short.Parse(dDtos[0].Road);
                res.state = true;
                return res;
            }
            return res;
        }

        //冻结车进道规则
        public RCRoadResModel IDC(RC_RuleDto dto, string area, string vin, string script, string tableName = "RC_Cache")
        {
            var res = new RCRoadResModel();
            var opDal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            //判断VIN是否为冻结车
            var dDtos = opDal.GetDtoList(string.Format("AND Area='{0}' AND VIN='{1}' AND State='3'", area, vin));
            if (dDtos.Count > 0)
            {
                var DbName = "◎ABP_Base◎";
                //查询冻结车道(已排除锁定车道)
                var djRoadSql = string.Format(DbName + "select road from {0} where Area = '{1}' and (PlaceState = '001' or PlaceState = '011') GROUP BY road", tableName,area);
                DataTable dt= SqlHelp.Query(djRoadSql).Tables[0];
                if(dt.Rows.Count > 0)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        //判断该车道是否已满
                        if (!MuzeyRCHelp.RoadIsFull(area, dr["road"].ToStr(), tableName))
                        {
                            res.state = true;
                            res.roadNum = short.Parse(dr[0].ToStr());
                            return res;
                        }
                    }
                }
            }
            return res;
        }

        //进道冻结规则
        public RCRoadResModel ID(RC_RuleDto dto, string area, string vin, string script, string tableName = "RC_Cache")
        {
            var res = new RCRoadResModel();
            var opDal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            //判断该车道是否已满如果满了直接GG
            if(MuzeyRCHelp.RoadIsFull(area, dto.Road, tableName))
            {
                res.state = false;
                res.msg = "该车道已满";
                return res;
            }

            //判断该车道进道是否锁定


            //判断VIN是否预冻结
            var dDtos = opDal.GetDtoList(string.Format("AND Area='{0}' AND VIN='{1}' AND State='7'", area, vin));
            //该VIN为预冻结
            if (dDtos.Count > 0)
            {
                res.roadNum = short.Parse(dto.Road);
                res.state = true;
                return res;
            }
            else
            {
                res.state = false;
                res.msg = "没有预锁定车辆";
                return res;
            }
        }

        //进道特征值匹配规则
        //FamilyCode=ABA&&FeatureCode=ABA FamilyCode=ABA||FeatureCode=ABA
        public RCRoadResModel IEQ(RC_RuleDto dto, string area, string vin, string script, string tableName = "RC_Cache")
        {
            var res = new RCRoadResModel();

            var dalCace = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dalCace.ChangeTableName(tableName);

            var server = new ACRcFictitiousAppService();
            var fData = server.GetFData(new MuzeyReqModel<ACRcFictitiousReqDto>() { datas = new List<ACRcFictitiousReqDto>() { new ACRcFictitiousReqDto() { area = area, VIN = vin } } });
            var ruleData = new MuzeyRCHelp().RcScriptPrase(script,"IEQ");
            if(CheckDicRule(ruleData.FamilyCodeCheckDic, fData.datas[0].FamilyCode) && CheckDicRule(ruleData.FeatureCodeCheckDic, fData.datas[0].FeatureCode))
            {
                res.state = true;
                res.roadNum = short.Parse(dto.Road);
            }

            return res;
        }

        //出道快速道规则
        public RCRoadResModel OSUP(RC_RuleDto dto, string area, string script, string tableName = "RC_Cache")
        {
            var res = new RCRoadResModel();
            //查询快速道是否有车
            var dal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dal.ChangeTableName(tableName);
            var cacheDto = dal.GetDtoList(string.Format("AND Area='{0}' AND IsSup='1' AND Place='01'", area) + whereStr)[0];
            if (!string.IsNullOrEmpty(cacheDto.VIN))
            {
                res.state = true;
                res.roadNum = short.Parse(cacheDto.Road);
            }
            return res;
        }

        //出道冻结规则
        public RCRoadResModel OD(RC_RuleDto dto, string area, string script, string tableName= "RC_Cache")
        {
            var res = new RCRoadResModel();
            //查询所有出道口锁定车辆
            var dal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
            dal.ChangeTableName(tableName);
            var cacheDtos = dal.GetDtoList(string.Format("AND Area='{0}' AND State='1' AND Place='01'", area) + whereStr);
            foreach(var cacheDto in cacheDtos)
            {
                res.state = true;
                res.roadNum = short.Parse(cacheDto.Road);
                res.isRev = true;
                return res;
            }
            return res;
        }

        //出道先进先出规则
        public RCRoadResModel OO(RC_RuleDto dto, string area, string script,string tableName = "RC_Cache")
        {
            var res = new RCRoadResModel();
            //查询最先进入的车
            var cacheDtos = SqlHelp.Query(string.Format("◎ABP_Base◎SELECT TOP 1 * FROM {1} WHERE 1=1 AND Area='{0}' AND State='0' AND Place='01' AND VIN <> '' AND VIN is not null" + whereStr + "order by seq", area, tableName)).Tables[0].DataTableToList<RC_CacheDto>();
            if(cacheDtos.Count > 0)
            {
                res.state = true;
                res.roadNum = short.Parse(cacheDtos[0].Road);
            }
            return res;
        }

        public bool CheckDicRule(Dictionary<string,string> dic, string data)
        {
            var res = new RCRoadResModel();
            foreach (var d in dic)
            {
                //包含
                if (data.Contains(d.Key))
                {
                    //=
                    if (d.Value[0] == '0')
                    {
                        //可破坏
                        if (d.Value[1] == '1')
                        {
                            return true;
                        }
                    }
                    //!=
                    else
                    {
                        //不可破坏
                        if (d.Value[1] == '0')
                        {
                            return false;
                        }
                    }
                }
                //不包含
                else
                {
                    //=
                    if (d.Value[0] == '0')
                    {
                        //不可破坏
                        if (d.Value[1] == '0')
                        {
                            return false;
                        }
                    }
                    //!=
                    else
                    {
                        //可破坏
                        if (d.Value[1] == '1')
                        {
                            return true;
                        }
                    }
                }
            }

            return true;
        }
    }

    public class RCRoadResModel
    {
        public RCRoadResModel()
        {
            state = false;
            roadNum = 0;
            isRev = false;
            isSub = false;
            msg = "没有理想车道";
        }

        public bool state { get; set; }
        public short roadNum { get; set; }
        public string msg { get; set; }
        public bool isRev { get; set; }
        public bool isSub { get; set; }
    }

    public class RCScriptModel
    {
        //val 匹配编码->(0:= 1:!=) 可破坏编码->(0:不可破坏 1:可破坏)
        public Dictionary<string, string> FamilyCodeCheckDic;
        public Dictionary<string, string> FeatureCodeCheckDic;

        public RCScriptModel()
        {
            FamilyCodeCheckDic = new Dictionary<string, string>();
            FeatureCodeCheckDic = new Dictionary<string, string>();
        }
    }
}
