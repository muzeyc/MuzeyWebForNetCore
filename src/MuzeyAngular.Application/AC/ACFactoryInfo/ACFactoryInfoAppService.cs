using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACFactoryInfoAppService
    {
        public MuzeyResModel<ACFactoryInfoResDto> GetDatas(MuzeyReqModel<ACFactoryInfoReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACFactoryInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_FACTORYDto>("ABP_Base");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACFactoryInfoResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACFactoryInfoResDto> GetData(MuzeyReqModel<ACFactoryInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACFactoryInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_FACTORYDto>("ABP_Base");
            var dataModel = new ACFactoryInfoResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new BASE_FACTORYDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACFactoryInfoResDto> Save(MuzeyReqModel<ACFactoryInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACFactoryInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_FACTORYDto>("ABP_Base");
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

        public MuzeyResModel<ACFactoryInfoResDto> Delete(MuzeyReqModel<ACFactoryInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACFactoryInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_FACTORYDto>("ABP_Base");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

