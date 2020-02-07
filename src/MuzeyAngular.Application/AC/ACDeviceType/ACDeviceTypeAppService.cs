using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACDeviceTypeAppService
    {
        public MuzeyResModel<ACDeviceTypeResDto> GetDatas(MuzeyReqModel<ACDeviceTypeReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACDeviceTypeResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_DEVICETYPEDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACDeviceTypeResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACDeviceTypeResDto> GetData(MuzeyReqModel<ACDeviceTypeReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACDeviceTypeResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_DEVICETYPEDto>(data.workShop + "※" + data.workShop + "_ANDON");
            var dataModel = new ACDeviceTypeResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new ALARM_DEVICETYPEDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACDeviceTypeResDto> Save(MuzeyReqModel<ACDeviceTypeReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACDeviceTypeResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_DEVICETYPEDto>(data.workShop + "※" + data.workShop + "_ANDON");
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

        public MuzeyResModel<ACDeviceTypeResDto> Delete(MuzeyReqModel<ACDeviceTypeReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACDeviceTypeResDto>();
            var dal = new MuzeyBusinessLogic<ALARM_DEVICETYPEDto>(data.workShop + "※" + data.workShop + "_ANDON");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

