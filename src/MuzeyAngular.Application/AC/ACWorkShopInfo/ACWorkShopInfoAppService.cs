using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACWorkShopInfoAppService
    {
        public MuzeyResModel<ACWorkShopInfoResDto> GetDatas(MuzeyReqModel<ACWorkShopInfoReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWorkShopInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_WORKSHOPDto>("ABP_Base");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACWorkShopInfoResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACWorkShopInfoResDto> GetData(MuzeyReqModel<ACWorkShopInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWorkShopInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_WORKSHOPDto>("ABP_Base");
            var dataModel = new ACWorkShopInfoResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new BASE_WORKSHOPDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACWorkShopInfoResDto> Save(MuzeyReqModel<ACWorkShopInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWorkShopInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_WORKSHOPDto>("ABP_Base");
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

        public MuzeyResModel<ACWorkShopInfoResDto> Delete(MuzeyReqModel<ACWorkShopInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWorkShopInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_WORKSHOPDto>("ABP_Base");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

