using BusinessLogic;
using CommonUtils;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACBypassAppService
    {
        public MuzeyResModel<ACBypassResDto> GetDtoData(MuzeyReqModel<ACBypassReqDto> reqModel, bool allFlag = false)
        {
            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACBypassResDto>();

            string dbName = "";
            dbName = filter.workShop + "※" + filter.workShop + "_ANDON";
            var dal = new MuzeyBusinessLogic<ANDON_QCOS_BYPASS_INFODto>(dbName);
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            List<ANDON_QCOS_BYPASS_INFODto> datas = null;
            if (allFlag)
            {
                datas = dal.GetDtoList(strWhere);
            }
            else
            {
                datas = dal.GetPageList(strWhere, "BypassStartTime DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            }

            resModel.totalCount = totalCount;
            foreach (var data in datas)
            {
                var rd = new ACBypassResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }
        public MuzeyResModel<ACBypassResDto> GetDatas(MuzeyReqModel<ACBypassReqDto> reqModel)
        {
            return GetDtoData(reqModel);
        }

        public MuzeyResModel<ACBypassResDto> Export(MuzeyReqModel<ACBypassReqDto> reqModel)
        {
            var req = reqModel.datas[0];
            var resModel = new MuzeyResModel<ACBypassResDto>();
            if (DateUtil.DateDiff(req.sTime.ToDateTime(), req.eTime.ToDateTime(), "Days") > 31 || string.IsNullOrEmpty(req.sTime) || string.IsNullOrEmpty(req.eTime))
            {
                resModel.CreateErr("只能导出时间段为1个月的数据！");
                return resModel;
            }
            var wb = ExcelUtil.ListToExcel(GetDtoData(reqModel, true).datas, reqModel.fileName.Split('.')[0], reqModel.cols);
            resModel.bs = new List<byte>(ExcelUtil.GetExcelBs(wb, reqModel.fileName));

            return resModel;
        }

        public MuzeyResModel<ACBypassResDto> GetData(MuzeyReqModel<ACBypassReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACBypassResDto>();

            string dbName = "";
            dbName = data.workShop + "※" + data.workShop + "_ANDON";
            var dal = new MuzeyBusinessLogic<ANDON_QCOS_BYPASS_INFODto>(dbName);
            var dataModel = new ACBypassResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new ANDON_QCOS_BYPASS_INFODto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACBypassResDto> Save(MuzeyReqModel<ACBypassReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACBypassResDto>();

            string dbName = "";
            dbName = data.workShop + "※" + data.workShop + "_ANDON";
            var dal = new MuzeyBusinessLogic<ANDON_QCOS_BYPASS_INFODto>(dbName);
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

        public MuzeyResModel<ACBypassResDto> Delete(MuzeyReqModel<ACBypassReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACBypassResDto>();

            string dbName = "";
            dbName = data.workShop + "※" + data.workShop + "_ANDON";
            var dal = new MuzeyBusinessLogic<ANDON_QCOS_BYPASS_INFODto>(dbName);
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

