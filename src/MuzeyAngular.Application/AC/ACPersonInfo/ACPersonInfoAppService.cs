using BusinessLogic;
using CommonUtils;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACPersonInfoAppService
    {
        public MuzeyResModel<ACPersonInfoResDto> GetDatas(MuzeyReqModel<ACPersonInfoReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACPersonInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_PERSONDto>("ABP_Base");
            var dalF = new MuzeyBusinessLogic<BASE_FACTORYDto>("ABP_Base");
            var dicF = dalF.GetDtoDic("", "FactoryCode");
            var dalWs = new MuzeyBusinessLogic<BASE_WORKSHOPDto>("ABP_Base");
            var dicWs = dalWs.GetDtoDic("", "WorkShopCode");
            var dalD = new MuzeyBusinessLogic<BASE_DEPARTMENTDto>("ABP_Base");
            var dicD = dalD.GetDtoDic("", "DepartmentCode");
            var dalA = new MuzeyBusinessLogic<AbpUserAccountsDto>("ABP_Base");
            var dicA = dalA.GetDtoDic("", "UserId");

            var dicP = new Dictionary<string, string>();
            dicP.Add("0", "一级管理员(无限制)");
            dicP.Add("1", "用户(车间+指定画面)");
            dicP.Add("2", "用户(指定画面)");
            dicP.Add("3", "二级管理员(车间+无限制)");
            dicP.Add("4", "二级管理员(部门+无限制)");

            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACPersonInfoResDto();
                ModelUtil.Copy(data, rd);
                rd.Factory = dicF[rd.Factory].FactoryName;
                rd.WorkShop = dicWs[rd.WorkShop].WorkShopName;
                rd.UserId = dicA[rd.UserId].UserName;
                rd.PermissionType = dicP[rd.PermissionType];
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACPersonInfoResDto> GetData(MuzeyReqModel<ACPersonInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACPersonInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_PERSONDto>("ABP_Base");
            var dataModel = new ACPersonInfoResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new BASE_PERSONDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACPersonInfoResDto> Save(MuzeyReqModel<ACPersonInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACPersonInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_PERSONDto>("ABP_Base");
            var dalPP = new MuzeyBusinessLogic<BASE_PERSON_PERMISSIONDto>("ABP_Base");

            if (string.IsNullOrEmpty(data.saveData.ID.ToStr()))
            {
                data.saveData.ID = dal.InsertEx(data.saveData);
            }
            else
            {
                dalPP.DeleteDto(new BASE_PERSON_PERMISSIONDto() { PersonID = data.saveData.ID.ToStr() });
                dal.UpdateDtoToPart(data.saveData);
            }

            if(data.saveData.PermissionType == "0")
            {
                return resModel;
            }
            var ppInsertList = new List<BASE_PERSON_PERMISSIONDto>();
            var removeViewData = GetViewRemoveData(data.saveData.WorkShop);
            if(data.saveData.PermissionType == "3" || data.saveData.PermissionType == "4")
            {
                foreach (var vs in data.Views.childItems)
                {
                    foreach (var v in vs.childItems)
                    {
                        if (!removeViewData.ContainsKey(v.name))
                        {
                            ppInsertList.Add(new BASE_PERSON_PERMISSIONDto()
                            {
                                PersonID = data.saveData.ID.ToStr()
                               ,
                                ViewName = v.name
                            });
                        }
                    }
                }
            }
            else
            {
                foreach (var vs in data.Views.childItems)
                {
                    foreach (var v in vs.childItems)
                    {
                        if (v.selected == "true" && !removeViewData.ContainsKey(v.name))
                        {
                            ppInsertList.Add(new BASE_PERSON_PERMISSIONDto()
                            {
                                PersonID = data.saveData.ID.ToStr()
                               ,
                                ViewName = v.name
                            });
                        }
                    }
                }
            }

            if (ppInsertList.Count > 0)
            {
                dalPP.InsertDtoList(ppInsertList);
            }

            return resModel;
        }

        public MuzeyResModel<ACPersonInfoResDto> Delete(MuzeyReqModel<ACPersonInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACPersonInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_PERSONDto>("ABP_Base");
            dal.DeleteDto(data.saveData);
            return resModel;
        }

        public MuzeyResModel<ACPersonInfoResDto> GetSelectData(MuzeyReqModel<ACPersonInfoReqDto> reqModel)
        {
            var resModel = new MuzeyResModel<ACPersonInfoResDto>();
            var rm = new ACPersonInfoResDto();
            var dalF = new MuzeyBusinessLogic<BASE_FACTORYDto>("ABP_Base");
            var dalWs = new MuzeyBusinessLogic<BASE_WORKSHOPDto>("ABP_Base");
            var dalD = new MuzeyBusinessLogic<BASE_DEPARTMENTDto>("ABP_Base");
            var dalA = new MuzeyBusinessLogic<AbpUserAccountsDto>("ABP_Base");
            var dalPP = new MuzeyBusinessLogic<BASE_PERSON_PERMISSIONDto>("ABP_Base");

            rm.fOps = dalF.GetSelectList("", BASE_FACTORYDto.DtoEnum.FactoryName.ToStr(), BASE_FACTORYDto.DtoEnum.FactoryCode.ToStr());
            rm.wsOps = dalWs.GetSelectList("", BASE_WORKSHOPDto.DtoEnum.WorkShopName.ToStr(), BASE_WORKSHOPDto.DtoEnum.WorkShopCode.ToStr());
            rm.dOps = dalD.GetSelectList("", BASE_DEPARTMENTDto.DtoEnum.DepartmentName.ToStr(), BASE_DEPARTMENTDto.DtoEnum.DepartmentCode.ToStr());
            rm.aOps = dalA.GetSelectList("AND IsDeleted=0", AbpUserAccountsDto.DtoEnum.UserName.ToStr(), AbpUserAccountsDto.DtoEnum.UserId.ToStr());
            var vDic = dalPP.GetDtoDic(string.Format(" AND PersonID='{0}'", reqModel.datas[0].saveData.ID), "ViewName");

            var menuDatas = new MuzeyAppService().GetMenuData(reqModel.datas[0].id);
            menuDatas.childItems.RemoveAt(0);
            menuDatas.childItems.RemoveAt(menuDatas.childItems.Count-1);
            menuDatas.childItems.RemoveAt(menuDatas.childItems.Count - 1);
            for (int i= 0;i<menuDatas.childItems.Count;i++)
            {
                for(int j=0;j< menuDatas.childItems[i].childItems.Count;j++)
                {
                    if (vDic.ContainsKey(menuDatas.childItems[i].childItems[j].name) || reqModel.datas[0].saveData.ID == null)
                    {
                        menuDatas.childItems[i].childItems[j].selected = "true";
                    }
                }
            }

            rm.Views = menuDatas;
            resModel.datas.Add(rm);
            return resModel;
        }

        public Dictionary<string,string> GetViewRemoveData(string workShop)
        {
            var baseDic = new Dictionary<string, Dictionary<string, string>>();
            var aes = new Dictionary<string,string>();
            aes.Add("冲压单批次", "冲压单批次");
            aes.Add("冲压订单", "冲压订单");
            aes.Add("铆接报警", "铆接报警");
            aes.Add("胶量低报警", "胶量低报警");
            aes.Add("冲压ADC换模记录", "冲压ADC换模记录");
            aes.Add("冲压ADC换模统计", "冲压ADC换模统计");
            aes.Add("冲压零件信息", "冲压零件信息");
            aes.Add("邮件服务配置", "邮件服务配置");
            aes.Add("收件人配置", "收件人配置");
            aes.Add("分线计划管理", "分线计划管理");
            baseDic.Add("AE", aes);

            var bes = new Dictionary<string, string>();
            bes.Add("冲压单批次", "冲压单批次");
            bes.Add("冲压订单", "冲压订单");
            bes.Add("拧紧枪结果", "拧紧枪结果");
            bes.Add("加注结果", "加注结果");
            bes.Add("冲压ADC换模记录", "冲压ADC换模记录");
            bes.Add("冲压ADC换模统计", "冲压ADC换模统计");
            bes.Add("冲压零件信息", "冲压零件信息");
            bes.Add("邮件服务配置", "邮件服务配置");
            bes.Add("收件人配置", "收件人配置");
            bes.Add("Bypass记录", "Bypass记录");
            baseDic.Add("BE", bes);

            var ses = new Dictionary<string, string>();
            ses.Add("上下线统计", "上下线统计");
            ses.Add("计划拉入拉出", "计划拉入拉出");
            ses.Add("过点记录", "过点记录");
            ses.Add("主线计划管理", "主线计划管理");
            ses.Add("分线计划管理", "分线计划管理");
            ses.Add("离线车辆", "离线车辆");
            ses.Add("计划产量", "计划产量");
            ses.Add("拧紧枪结果", "拧紧枪结果");
            ses.Add("加注结果", "加注结果");
            ses.Add("拧紧枪报警", "拧紧枪报警");
            ses.Add("铆接报警", "铆接报警");
            ses.Add("胶量低报警", "胶量低报警");
            ses.Add("物料安灯记录", "物料安灯记录");
            ses.Add("邮件服务配置", "邮件服务配置");
            ses.Add("收件人配置", "收件人配置");
            ses.Add("Bypass记录", "Bypass记录");
            baseDic.Add("SE", ses);

            var alls = new Dictionary<string, string>();
            alls.Add("拧紧枪结果", "拧紧枪结果"); 
            alls.Add("Bypass记录", "Bypass记录");
            alls.Add("加注结果", "加注结果");
            alls.Add("冲压ADC换模记录", "冲压ADC换模记录");
            alls.Add("冲压ADC换模统计", "冲压ADC换模统计");
            baseDic.Add("Logistics", alls);

            return baseDic[workShop];
        }
    }
}

