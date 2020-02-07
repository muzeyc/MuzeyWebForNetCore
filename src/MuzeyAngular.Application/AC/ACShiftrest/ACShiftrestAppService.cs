using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACShiftrestAppService
    {
        public MuzeyResModel<ACShiftrestResDto> GetDatas(MuzeyReqModel<ACShiftrestReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACShiftrestResDto>();
            var dbName = "";
            if (filter.workShop == "SE")
            {
                dbName = filter.workShop + "※" + filter.workShop + "_ANDON";
            }
            else
            {
                dbName = filter.workShop + "※" + filter.workShop + "_AVI";
            }
            var dal = new MuzeyBusinessLogic<AVI_SHIFTRESTDto>(dbName);
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "WorkDay DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACShiftrestResDto();
                ModelUtil.Copy(data, rd);
                rd.crossDayName = data.CrossDay == 1 ? "是" : "否";
                rd.workDay = data.WorkDay.ToDateTime().ToString("yyyy-MM-dd");
                rd.sT = data.BeginTime.ToDateTime().ToString("HH:mm");
                rd.eT = data.EndTime.ToDateTime().ToString("HH:mm");
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACShiftrestResDto> GetData(MuzeyReqModel<ACShiftrestReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACShiftrestResDto>();
            var dbName = "";
            if (data.workShop == "SE")
            {
                dbName = data.workShop + "※" + data.workShop + "_ANDON";
            }
            else
            {
                dbName = data.workShop + "※" + data.workShop + "_AVI";
            }
            var dal = new MuzeyBusinessLogic<AVI_SHIFTRESTDto>(dbName);
            var dataModel = new ACShiftrestResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new AVI_SHIFTRESTDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACShiftrestResDto> Save(MuzeyReqModel<ACShiftrestReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACShiftrestResDto>();
            var dbName = "";
            if (data.workShop == "SE")
            {
                dbName = data.workShop + "※" + data.workShop + "_ANDON";
            }
            else
            {
                dbName = data.workShop + "※" + data.workShop + "_AVI";
            }
            var dal = new MuzeyBusinessLogic<AVI_SHIFTRESTDto>(dbName);
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

        public MuzeyResModel<ACShiftrestResDto> Delete(MuzeyReqModel<ACShiftrestReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACShiftrestResDto>();
            var dbName = "";
            if (data.workShop == "SE")
            {
                dbName = data.workShop + "※" + data.workShop + "_ANDON";
            }
            else
            {
                dbName = data.workShop + "※" + data.workShop + "_AVI";
            }
            var dal = new MuzeyBusinessLogic<AVI_SHIFTRESTDto>(dbName);
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

