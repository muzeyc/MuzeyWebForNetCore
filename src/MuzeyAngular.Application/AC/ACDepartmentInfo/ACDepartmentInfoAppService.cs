using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACDepartmentInfoAppService
    {
        public MuzeyResModel<ACDepartmentInfoResDto> GetDatas(MuzeyReqModel<ACDepartmentInfoReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACDepartmentInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_DEPARTMENTDto>("ABP_Base");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "ID", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACDepartmentInfoResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACDepartmentInfoResDto> GetData(MuzeyReqModel<ACDepartmentInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACDepartmentInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_DEPARTMENTDto>("ABP_Base");
            var dataModel = new ACDepartmentInfoResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new BASE_DEPARTMENTDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACDepartmentInfoResDto> Save(MuzeyReqModel<ACDepartmentInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACDepartmentInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_DEPARTMENTDto>("ABP_Base");
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

        public MuzeyResModel<ACDepartmentInfoResDto> Delete(MuzeyReqModel<ACDepartmentInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACDepartmentInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_DEPARTMENTDto>("ABP_Base");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

