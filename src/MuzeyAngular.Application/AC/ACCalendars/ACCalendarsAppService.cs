using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACCalendarsAppService
    {
        public MuzeyResModel<ACCalendarsResDto> GetDatas(MuzeyReqModel<ACCalendarsReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACCalendarsResDto>();
            var dbName = "";
            if (filter.workShop == "SE")
            {
                dbName = "SE※" + filter.workShop + "_ANDON";
            }
            else
            {
                dbName = filter.workShop + "※" + filter.workShop + "_AVI";
            }
            var dal = new MuzeyBusinessLogic<AVI_CALENDARSDto>(dbName);
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "WorkDay DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACCalendarsResDto();
                ModelUtil.Copy(data, rd);
                rd.workDay = data.WorkDay.ToDateTime().ToString("yyyy-MM-dd");
                rd.sT = data.BeginTime.ToDateTime().ToString("HH:mm");
                rd.eT = data.EndTime.ToDateTime().ToString("HH:mm");
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACCalendarsResDto> GetData(MuzeyReqModel<ACCalendarsReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACCalendarsResDto>();
            var dbName = "";
            if (data.workShop == "SE")
            {
                dbName = "SE※" + data.workShop + "_ANDON";
            }
            else
            {
                dbName = data.workShop + "※" + data.workShop + "_AVI";
            }
            var dal = new MuzeyBusinessLogic<AVI_CALENDARSDto>(dbName);
            var dataModel = new ACCalendarsResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new AVI_CALENDARSDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACCalendarsResDto> Save(MuzeyReqModel<ACCalendarsReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACCalendarsResDto>();
            var dbName = "";
            if (data.workShop == "SE")
            {
                dbName = "SE※" + data.workShop + "_ANDON";
            }
            else
            {
                dbName = data.workShop + "※" + data.workShop + "_AVI";
            }
            var dal = new MuzeyBusinessLogic<AVI_CALENDARSDto>(dbName);
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

        public MuzeyResModel<ACCalendarsResDto> Delete(MuzeyReqModel<ACCalendarsReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACCalendarsResDto>();
            var dbName = "";
            if (data.workShop == "SE")
            {
                dbName = "SE※" + data.workShop + "_ANDON";
            }
            else
            {
                dbName = data.workShop + "※" + data.workShop + "_AVI";
            }
            var dal = new MuzeyBusinessLogic<AVI_CALENDARSDto>(dbName);
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

