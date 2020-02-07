using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACAlarmSystemAppService
    {
        public MuzeyResModel<ACAlarmSystemResDto> GetDatas(MuzeyReqModel<ACAlarmSystemReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACAlarmSystemResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_SYSTEMDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACAlarmSystemResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACAlarmSystemResDto> GetData(MuzeyReqModel<ACAlarmSystemReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACAlarmSystemResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_SYSTEMDto>(data.workShop + "※" + data.workShop + "_ANDON");
            var dataModel = new ACAlarmSystemResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new ALARM_SYSTEMDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACAlarmSystemResDto> Save(MuzeyReqModel<ACAlarmSystemReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACAlarmSystemResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_SYSTEMDto>(data.workShop + "※" + data.workShop + "_ANDON");
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

        public MuzeyResModel<ACAlarmSystemResDto> Delete(MuzeyReqModel<ACAlarmSystemReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACAlarmSystemResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_SYSTEMDto>(data.workShop + "※" + data.workShop + "_ANDON");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

