using BusinessLogic;
using CommonUtils;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACOfflineCarAppService
    {
        public MuzeyResModel<ACOfflineCarResDto> GetDatas(MuzeyReqModel<ACOfflineCarReqDto> reqModel)
        {
            var replaceColDic = new Dictionary<string, Dictionary<string, string>>();

            var aeCol = new Dictionary<string, string>();
            aeCol.Add("BECarType", ""); 
            aeCol.Add("BodySelCode", "");
            aeCol.Add("BEOnSeq", "");
            aeCol.Add("PEOnSeq", "");

            var beCol = new Dictionary<string, string>();
            beCol.Add("AECarType", "");

            replaceColDic.Add("AE", aeCol);
            replaceColDic.Add("BE", beCol);

            var dicOperMode = new Dictionary<string, string>();
            dicOperMode.Add("A","新增");
            dicOperMode.Add("C", "修改");
            dicOperMode.Add("D", "删除");
            dicOperMode.Add("E", "报废");

            var filter = reqModel.datas[0];
            filter.state = "1";
            var resModel = new MuzeyResModel<ACOfflineCarResDto>();
            var dal = new MuzeyBusinessLogic<AVI_SETIN_SETOUTDto>(filter.workShop + "※" + filter.workShop + "_AVI");
            if (replaceColDic.ContainsKey(filter.workShop))
            {
                dal.ReplaceCol(replaceColDic[filter.workShop]);
            }
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "SetOutTime DESC", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACOfflineCarResDto();
                ModelUtil.Copy(data, rd);
                if (dicOperMode.ContainsKey(rd.OperMode))
                {
                    rd.OperMode = dicOperMode[rd.OperMode];
                }
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACOfflineCarResDto> GetData(MuzeyReqModel<ACOfflineCarReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACOfflineCarResDto>();
            var dal = new MuzeyBusinessLogic<AVI_SETIN_SETOUTDto>(data.workShop + "※" + data.workShop + "_AVI");
            var dataModel = new ACOfflineCarResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new AVI_SETIN_SETOUTDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACOfflineCarResDto> Save(MuzeyReqModel<ACOfflineCarReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACOfflineCarResDto>();
            var dal = new MuzeyBusinessLogic<AVI_SETIN_SETOUTDto>(data.workShop + "※" + data.workShop + "_AVI");
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

        public MuzeyResModel<ACOfflineCarResDto> Delete(MuzeyReqModel<ACOfflineCarReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACOfflineCarResDto>();
            var dal = new MuzeyBusinessLogic<AVI_SETIN_SETOUTDto>(data.workShop + "※" + data.workShop + "_AVI");
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

