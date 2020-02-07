using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACSEPlanOrderAppService
    {
        public MuzeyResModel<ACSEPlanOrderResDto> GetDatas(MuzeyReqModel<ACSEPlanOrderReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACSEPlanOrderResDto>();
            var dal = new MuzeyBusinessLogic<AVI_PLANORDERDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "PlanDate,SEOnSeq", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACSEPlanOrderResDto();
                ModelUtil.Copy(data, rd);
                rd.FreezeState = data.FreezeState == "1" ? "冻结" : "正常";
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACSEPlanOrderResDto> GetData(MuzeyReqModel<ACSEPlanOrderReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACSEPlanOrderResDto>();
            var dal = new MuzeyBusinessLogic<AVI_PLANORDERDto>(data.workShop + "※" + data.workShop + "_ANDON");
            var dataModel = new ACSEPlanOrderResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new AVI_PLANORDERDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACSEPlanOrderResDto> Save(MuzeyReqModel<ACSEPlanOrderReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACSEPlanOrderResDto>();
            var dal = new MuzeyBusinessLogic<AVI_PLANORDERDto>("SE※SE_ANDON");
            if (string.IsNullOrEmpty(data.saveData.ID.ToStr()))
            {
                dal.InsertDto(data.saveData);
            }
            else
            {
                data.saveData.PlanDate = data.saveData.PlanDate.ToDateTime().ToString("yyyy-MM-dd");
                //data.saveData.FreezeState = "0";
                var dtoList = dal.GetDtoList(string.Format("AND PlanDate='{0}'", data.saveData.PlanDate));
                for(int i= 0;i< dtoList.Count;i++)
                {
                    if(data.saveData.SEOnSeq.ToInt() <= dtoList[i].SEOnSeq.ToInt())
                    {
                        dtoList[i].SEOnSeq = (dtoList[i].SEOnSeq.ToInt() + 1).ToStr();
                    }
                }
                dtoList.Add(data.saveData);
                dal.UpdateDtoListToPart(dtoList);
            }
            return resModel;
        }
    }
}

