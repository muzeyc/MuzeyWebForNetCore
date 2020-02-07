using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACLineAppService
    {
        public MuzeyResModel<ACLineResDto> GetDatas(MuzeyReqModel<ACLineReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACLineResDto>();
            var dal = new MuzeyBusinessLogic<BASE_LINEDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "LineCode", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACLineResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACLineResDto> GetData(MuzeyReqModel<ACLineReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACLineResDto>();
            var dal = new MuzeyBusinessLogic<BASE_LINEDto>(data.workShop + "※" + data.workShop + "_ANDON");
            var dataModel = new ACLineResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new BASE_LINEDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACLineResDto> Save(MuzeyReqModel<ACLineReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACLineResDto>();
            var dal = new MuzeyBusinessLogic<BASE_LINEDto>(data.workShop + "※" + data.workShop + "_ANDON");
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

        public MuzeyResModel<ACLineResDto> Delete(MuzeyReqModel<ACLineReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACLineResDto>();
            var dal = new MuzeyBusinessLogic<BASE_LINEDto>(data.workShop + "※" + data.workShop + "_ANDON");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

