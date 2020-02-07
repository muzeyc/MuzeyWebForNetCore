using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACMCConfigAppService
    {
        public MuzeyResModel<ACMCConfigResDto> GetDatas(MuzeyReqModel<ACMCConfigReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMCConfigResDto>();
            var dal = new MuzeyBusinessLogic<AVI_MATERIELCALL_CONFIG_MQDto>(filter.workShop + "※" + filter.workShop + "_AVI");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACMCConfigResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACMCConfigResDto> GetData(MuzeyReqModel<ACMCConfigReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMCConfigResDto>();
            var dal = new MuzeyBusinessLogic<AVI_MATERIELCALL_CONFIG_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            var dataModel = new ACMCConfigResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new AVI_MATERIELCALL_CONFIG_MQDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACMCConfigResDto> Save(MuzeyReqModel<ACMCConfigReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMCConfigResDto>();
            var dal = new MuzeyBusinessLogic<AVI_MATERIELCALL_CONFIG_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
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

        public MuzeyResModel<ACMCConfigResDto> Delete(MuzeyReqModel<ACMCConfigReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACMCConfigResDto>();
            var dal = new MuzeyBusinessLogic<AVI_MATERIELCALL_CONFIG_MQDto>(data.workShop + "※" + data.workShop + "_AVI");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

