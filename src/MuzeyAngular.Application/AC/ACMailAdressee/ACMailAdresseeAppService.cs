using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACMailAdresseeAppService
    {
        public MuzeyResModel<ACMailAdresseeResDto> GetDatas(MuzeyReqModel<ACMailAdresseeReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMailAdresseeResDto>();
            var dal = new MuzeyBusinessLogic<MAIL_ADRESSEEDto>("ABP_Base");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "Adressee,WorkShop,AlarmTypeCode", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACMailAdresseeResDto();
                ModelUtil.Copy(data, rd);
                rd.AdresseeStateStr = data.AdresseeState == 0 ? "不发送" : "发送";
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACMailAdresseeResDto> GetData(MuzeyReqModel<ACMailAdresseeReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMailAdresseeResDto>();
            var dal = new MuzeyBusinessLogic<MAIL_ADRESSEEDto>("ABP_Base");
            var dataModel = new ACMailAdresseeResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new MAIL_ADRESSEEDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACMailAdresseeResDto> Save(MuzeyReqModel<ACMailAdresseeReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMailAdresseeResDto>();
            var dal = new MuzeyBusinessLogic<MAIL_ADRESSEEDto>("ABP_Base");
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

        public MuzeyResModel<ACMailAdresseeResDto> Delete(MuzeyReqModel<ACMailAdresseeReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMailAdresseeResDto>();
            var dal = new MuzeyBusinessLogic<MAIL_ADRESSEEDto>("ABP_Base");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

