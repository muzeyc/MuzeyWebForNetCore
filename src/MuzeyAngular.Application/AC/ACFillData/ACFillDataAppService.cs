using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACFillDataAppService
    {
        public MuzeyResModel<ACFillDataResDto> GetDatas(MuzeyReqModel<ACFillDataReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACFillDataResDto>();
            var dal = new MuzeyBusinessLogic<FILLDATA_INFODto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "VIN,InsertTime", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACFillDataResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACFillDataResDto> GetData(MuzeyReqModel<ACFillDataReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACFillDataResDto>();
            var dal = new MuzeyBusinessLogic<FILLDATA_INFODto>(data.workShop + "※" + data.workShop + "_ANDON");
            var dataModel = new ACFillDataResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new FILLDATA_INFODto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACFillDataResDto> Save(MuzeyReqModel<ACFillDataReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACFillDataResDto>();
            var dal = new MuzeyBusinessLogic<FILLDATA_INFODto>(data.workShop + "※" + data.workShop + "_ANDON");
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

        public MuzeyResModel<ACFillDataResDto> Delete(MuzeyReqModel<ACFillDataReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACFillDataResDto>();
            var dal = new MuzeyBusinessLogic<FILLDATA_INFODto>(data.workShop + "※" + data.workShop + "_ANDON");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

