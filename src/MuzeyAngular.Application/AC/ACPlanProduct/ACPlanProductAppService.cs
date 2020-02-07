using BusinessLogic;
using CommonUtils;
using System;

namespace MuzeyServer
{
    public class ACPlanProductAppService
    {
        public MuzeyResModel<ACPlanProductResDto> GetDatas(MuzeyReqModel<ACPlanProductReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACPlanProductResDto>();
            var dal = new MuzeyBusinessLogic<BASE_PLAN_PRODUCTDto>("ABP_Base");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACPlanProductResDto();
                ModelUtil.Copy(data, rd);
                rd.PlanTimeS = data.PlanTimeS.Split(' ')[1];
                rd.PlanTimeE = data.PlanTimeE.Split(' ')[1];
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACPlanProductResDto> GetData(MuzeyReqModel<ACPlanProductReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACPlanProductResDto>();
            var dal = new MuzeyBusinessLogic<BASE_PLAN_PRODUCTDto>("ABP_Base");
            var dataModel = new ACPlanProductResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new BASE_PLAN_PRODUCTDto() { ID = data.saveData.ID }), dataModel);
            dataModel.PlanTimeS = dataModel.PlanTimeS.Split(' ')[1];
            dataModel.PlanTimeE = dataModel.PlanTimeE.Split(' ')[1];
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACPlanProductResDto> GetTime(MuzeyReqModel<ACPlanProductReqDto> reqModel)
        {

            var data = reqModel.datas[0];
            var dbName = "";
            if (data.workShop == "SE")
            {
                dbName = data.workShop + "※" + data.workShop + "_ANDON";
            }
            else
            {
                dbName = data.workShop + "※" + data.workShop + "_AVI";
            }
            var resModel = new MuzeyResModel<ACPlanProductResDto>();
            var dal = new MuzeyBusinessLogic<AVI_CALENDARSDto>(dbName);
            var req = new ACShiftrestReqDto();
            req.sTime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            req.eTime = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            req.workShop = data.workShop;
            var dtos = dal.GetDtoList(MuzeyReqUtil.GetSqlWhere(req));
            AVI_CALENDARSDto dto = null;
            if (dtos.Count > 0)
            {
                dto = dtos[0];
                var dataModel = new ACPlanProductResDto();
                dataModel.ShiftrestName = dto.ShiftName;
                dataModel.WorkShop = data.workShop;
                dataModel.PlanDate = dto.WorkDay.ToDateTime().ToString("yyyy-MM-dd");
                dataModel.PlanTimeS = dto.BeginTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                dataModel.PlanTimeE = dto.EndTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                dataModel.PlanTimeS = dataModel.PlanTimeS.Split(' ')[1];
                dataModel.PlanTimeE = dataModel.PlanTimeE.Split(' ')[1];
                resModel.datas.Add(dataModel);
            }
            else
            {
                var dataModel = new ACPlanProductResDto();
                dataModel.WorkShop = data.workShop;
                resModel.datas.Add(dataModel);
            }
            
            return resModel;
        }

        public MuzeyResModel<ACPlanProductResDto> Save(MuzeyReqModel<ACPlanProductReqDto> reqModel)
        {

            var data = reqModel.datas[0];
            var dateNow = DateTime.Now;
            var resModel = new MuzeyResModel<ACPlanProductResDto>();
            var dal = new MuzeyBusinessLogic<BASE_PLAN_PRODUCTDto>("ABP_Base");
            data.saveData.PlanDate = data.saveData.PlanDate.ToDateTime().ToString("yyyy-MM-dd");
            if (data.saveData.PlanTimeS.Replace(":","").ToInt() > data.saveData.PlanTimeE.Replace(":", "").ToInt())
            {
                data.saveData.PlanTimeE = dateNow.AddDays(1).ToString("yyyy-MM-dd") + " " + data.saveData.PlanTimeE;
            }
            else
            {
                data.saveData.PlanTimeE = dateNow.ToString("yyyy-MM-dd") + " " + data.saveData.PlanTimeE;
            }
            data.saveData.PlanTimeS = dateNow.ToString("yyyy-MM-dd") + " " + data.saveData.PlanTimeS;
            if(data.saveData.WorkShop == "AE")
            {
                data.saveData.LineMain = "";
                data.saveData.LineBranch = "";
                data.saveData.CarType = "";
            }
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

        public MuzeyResModel<ACPlanProductResDto> Delete(MuzeyReqModel<ACPlanProductReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACPlanProductResDto>();
            var dal = new MuzeyBusinessLogic<BASE_PLAN_PRODUCTDto>("ABP_Base");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

