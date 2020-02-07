using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACMailBaseConfigAppService
    {
        public MuzeyResModel<ACMailBaseConfigResDto> GetDatas(MuzeyReqModel<ACMailBaseConfigReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMailBaseConfigResDto>();
            var dal = new MuzeyBusinessLogic<MAIL_BASECONFIGDto>("ABP_Base");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACMailBaseConfigResDto();
                ModelUtil.Copy(data, rd);
                rd.PrecautiousT = data.PrecautiousTime;
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACMailBaseConfigResDto> GetData(MuzeyReqModel<ACMailBaseConfigReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMailBaseConfigResDto>();
            var dal = new MuzeyBusinessLogic<MAIL_BASECONFIGDto>("ABP_Base");
            var dataModel = new ACMailBaseConfigResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new MAIL_BASECONFIGDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACMailBaseConfigResDto> Save(MuzeyReqModel<ACMailBaseConfigReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMailBaseConfigResDto>();
            var dal = new MuzeyBusinessLogic<MAIL_BASECONFIGDto>("ABP_Base");
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

        public MuzeyResModel<ACMailBaseConfigResDto> Delete(MuzeyReqModel<ACMailBaseConfigReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMailBaseConfigResDto>();
            var dal = new MuzeyBusinessLogic<MAIL_BASECONFIGDto>("ABP_Base");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

