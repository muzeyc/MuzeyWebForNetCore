using BusinessLogic;
using CommonUtils;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACRCRuleAppService
    {
        public Dictionary<string, string> roOps;
        public Dictionary<string, string> ioOps;
        public Dictionary<string, string> idOps;
        public Dictionary<string, string> ieOps;

        public ACRCRuleAppService()
        {
            roOps = new Dictionary<string, string>();
            ioOps = new Dictionary<string, string>();
            idOps = new Dictionary<string, string>();
            ieOps = new Dictionary<string, string>();
            roOps.Add("01", "1道");
            roOps.Add("02", "2道");
            roOps.Add("03", "3道");
            roOps.Add("04", "4道");
            roOps.Add("05", "5道");
            roOps.Add("06", "6道");
            roOps.Add("07", "7道");
            roOps.Add("08", "8道");
            roOps.Add("09", "9道");
            roOps.Add("10", "10道");

            ioOps.Add("1", "进道规则");
            ioOps.Add("2", "出道规则");

            idOps.Add("0","不可破坏");
            idOps.Add("1", "可破坏");

            ieOps.Add("0","不启用");
            ieOps.Add("1", "启用");
            ieOps.Add("2", "核心启用");
        }

        public MuzeyResModel<ACRCRuleResDto> GetDatas(MuzeyReqModel<ACRCRuleReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRCRuleResDto>();
            var dal = new MuzeyBusinessLogic<RC_RuleDto>("ABP_Base");
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "Area,InOutType,Seq", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACRCRuleResDto();
                ModelUtil.Copy(data, rd);
                rd.Road = roOps.ContainsKey(rd.Road) ? roOps[rd.Road] : rd.Road;
                rd.InOutType = ioOps.ContainsKey(rd.InOutType) ? ioOps[rd.InOutType] : rd.InOutType;
                rd.IsDestroy = idOps.ContainsKey(rd.IsDestroy) ? idOps[rd.IsDestroy] : rd.IsDestroy;
                rd.IsEnable = ieOps.ContainsKey(rd.IsEnable) ? ieOps[rd.IsEnable] : rd.IsEnable;
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACRCRuleResDto> GetData(MuzeyReqModel<ACRCRuleReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRCRuleResDto>();
            var dal = new MuzeyBusinessLogic<RC_RuleDto>("ABP_Base");
            var dataModel = new ACRCRuleResDto();
            ModelUtil.Copy(dal.GetDtoByPK(new RC_RuleDto() { ID = data.saveData.ID }), dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACRCRuleResDto> Save(MuzeyReqModel<ACRCRuleReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRCRuleResDto>();
            var dal = new MuzeyBusinessLogic<RC_RuleDto>("ABP_Base");
            var dtoList = dal.GetDtoList(string.Format("AND Area='{0}' AND InOutType='{1}' order by seq", data.saveData.Area, data.saveData.InOutType));
            for (int i = 0; i < dtoList.Count; i++)
            {
                if (!string.IsNullOrEmpty(data.saveData.ID.ToStr()))
                {
                    if(data.saveData.ID == dtoList[i].ID && data.saveData.Seq == dtoList[i].Seq)
                    {
                        break;
                    }
                }

                if (data.saveData.Seq.ToInt() <= dtoList[i].Seq.ToInt())
                {
                    dtoList[i].Seq = (dtoList[i].Seq.ToInt() + 1).ToStr();
                }
            }
            dal.UpdateDtoListToPart(dtoList);
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

        public MuzeyResModel<ACRCRuleResDto> Delete(MuzeyReqModel<ACRCRuleReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACRCRuleResDto>();
            var dal = new MuzeyBusinessLogic<RC_RuleDto>("ABP_Base");
            var dto = dal.GetDtoByPK(data.saveData);
            dal.DeleteDto(data.saveData);

            var dtoList = dal.GetDtoList(string.Format("AND Area='{0}' AND InOutType='{1}' order by seq", dto.Area, dto.InOutType));
            int seq = 1;
            for(int i=0;i< dtoList.Count;i++)
            {
                dtoList[i].Seq = seq.ToStr();
                seq++;
            }
            dal.UpdateDtoListToPart(dtoList);
            return resModel;
        }
    }
}

