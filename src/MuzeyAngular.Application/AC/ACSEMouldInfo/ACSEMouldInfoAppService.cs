using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACSEMouldInfoAppService
    {
        public MuzeyResModel<ACSEMouldInfoResDto> GetDatas(MuzeyReqModel<ACSEMouldInfoReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACSEMouldInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_MOULDINFODto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "MouldCode", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACSEMouldInfoResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACSEMouldInfoResDto> GetData(MuzeyReqModel<ACSEMouldInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACSEMouldInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_MOULDINFODto>(data.workShop + "※" + data.workShop + "_ANDON");
            var dataModel = new ACSEMouldInfoResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new BASE_MOULDINFODto() { MouldCode = data.saveData.MouldCode }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACSEMouldInfoResDto> Save(MuzeyReqModel<ACSEMouldInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACSEMouldInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_MOULDINFODto>(data.workShop + "※" + data.workShop + "_ANDON");
            if (data.op == "add")
            {
                dal.InsertDto(data.saveData);
            }
            else
            {
                dal.UpdateDtoToPart(data.saveData);
            }
            return resModel;
        }

        public MuzeyResModel<ACSEMouldInfoResDto> Delete(MuzeyReqModel<ACSEMouldInfoReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACSEMouldInfoResDto>();
            var dal = new MuzeyBusinessLogic<BASE_MOULDINFODto>(data.workShop + "※" + data.workShop + "_ANDON");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

