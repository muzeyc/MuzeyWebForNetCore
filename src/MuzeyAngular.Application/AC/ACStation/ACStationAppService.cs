using BusinessLogic;
using Castle.Core.Logging;
using CommonUtils;

namespace MuzeyServer
{
    public class ACStationAppService
    {
        public MuzeyResModel<ACStationResDto> GetDatas(MuzeyReqModel<ACStationReqDto> reqModel)
        {
            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACStationResDto>();
            var dal = new MuzeyBusinessLogic<BASE_STATIONDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "StationCode", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACStationResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACStationResDto> GetData(MuzeyReqModel<ACStationReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACStationResDto>();
            var dal = new MuzeyBusinessLogic<BASE_STATIONDto>(data.workShop + "※" + data.workShop + "_ANDON");
            var dataModel = new ACStationResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new BASE_STATIONDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACStationResDto> Save(MuzeyReqModel<ACStationReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACStationResDto>();
            var dal = new MuzeyBusinessLogic<BASE_STATIONDto>(data.workShop + "※" + data.workShop + "_ANDON");
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

        public MuzeyResModel<ACStationResDto> Delete(MuzeyReqModel<ACStationReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACStationResDto>();
            var dal = new MuzeyBusinessLogic<BASE_STATIONDto>(data.workShop + "※" + data.workShop + "_ANDON");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

