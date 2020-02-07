using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACAlarmTypeAppService
    {
        public MuzeyResModel<ACAlarmTypeResDto> GetDatas(MuzeyReqModel<ACAlarmTypeReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACAlarmTypeResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_ALARMTYPEDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACAlarmTypeResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACAlarmTypeResDto> GetData(MuzeyReqModel<ACAlarmTypeReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACAlarmTypeResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_ALARMTYPEDto>(data.workShop + "※" + data.workShop + "_ANDON");
            var dataModel = new ACAlarmTypeResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new ALARM_ALARMTYPEDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACAlarmTypeResDto> Save(MuzeyReqModel<ACAlarmTypeReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACAlarmTypeResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_ALARMTYPEDto>(data.workShop + "※" + data.workShop + "_ANDON");
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

        public MuzeyResModel<ACAlarmTypeResDto> Delete(MuzeyReqModel<ACAlarmTypeReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACAlarmTypeResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_ALARMTYPEDto>(data.workShop + "※" + data.workShop + "_ANDON");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

