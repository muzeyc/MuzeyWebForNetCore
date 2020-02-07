using BusinessLogic;
using CommonUtils;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACRCLogAppService
    {
        public Dictionary<string, string> stateDic;
        public ACRCLogAppService()
        {
            stateDic = new Dictionary<string, string>();
            stateDic.Add("1","进道");
            stateDic.Add("2", "出道");
            stateDic.Add("4", "车辆解冻");
            stateDic.Add("5", "车道冻结");
            stateDic.Add("6", "车道解冻");
            stateDic.Add("7", "车辆预冻结");
            stateDic.Add("8", "车辆预冻结完成");
            stateDic.Add("9", "车辆预约快速道");
            stateDic.Add("A", "进道预设");
        }

        public MuzeyResModel<ACRCLogResDto> GetDatas(MuzeyReqModel<ACRCLogReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRCLogResDto>();
            var dal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            if (!string.IsNullOrEmpty(filter.workShop))
            {
                strWhere += string.Format(" AND Area='{0}'", filter.workShop);
            }
            var datas = dal.GetPageList(strWhere + " AND State not in('1','2','3','4','5','6','B','C','D','E')", "OpTime DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACRCLogResDto();
                ModelUtil.Copy(data, rd);
                rd.Road = rd.Road.PadLeft(2,'0');
                rd.State = stateDic[rd.State];
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACRCLogResDto> GetData(MuzeyReqModel<ACRCLogReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRCLogResDto>();
            var dal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            var dataModel = new ACRCLogResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new RC_InOutLogDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACRCLogResDto> Save(MuzeyReqModel<ACRCLogReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRCLogResDto>();
            var dal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
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

        public MuzeyResModel<ACRCLogResDto> Delete(MuzeyReqModel<ACRCLogReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRCLogResDto>();
            var dal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

