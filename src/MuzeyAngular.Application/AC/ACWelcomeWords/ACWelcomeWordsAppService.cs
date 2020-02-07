using BusinessLogic;
using CommonUtils;

namespace MuzeyServer
{
    public class ACWelcomeWordsAppService
    {
        public MuzeyResModel<ACWelcomeWordsResDto> GetDatas(MuzeyReqModel<ACWelcomeWordsReqDto> reqModel)
        {

            var filter = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWelcomeWordsResDto>();

            string dbName = "";
            dbName = filter.workShop + "※" + filter.workShop + "_ANDON";
            var dal = new MuzeyBusinessLogic<ANDON_WelcomeWordsDto>(dbName);
            var totalCount = 0;
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            var datas = dal.GetPageList(strWhere, "Chinese", reqModel.offset, reqModel.pageSize, out totalCount);
            resModel.totalCount = totalCount;
            foreach(var data in datas)
            {
                var rd = new ACWelcomeWordsResDto();
                ModelUtil.Copy(data, rd);
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<ACWelcomeWordsResDto> GetData(MuzeyReqModel<ACWelcomeWordsReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWelcomeWordsResDto>();

            string dbName = "";
            dbName = data.workShop + "※" + data.workShop + "_ANDON";
            var dal = new MuzeyBusinessLogic<ANDON_WelcomeWordsDto>(dbName);
            var dataModel = new ACWelcomeWordsResDto();
            ModelUtil.Copy(dal.GetDtoList("")[0], dataModel);
            resModel.datas.Add(dataModel);
            return resModel;
        }

        public MuzeyResModel<ACWelcomeWordsResDto> Save(MuzeyReqModel<ACWelcomeWordsReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWelcomeWordsResDto>();

            string dbName = "";
            dbName = data.workShop + "※" + data.workShop + "_ANDON";
            var dal = new MuzeyBusinessLogic<ANDON_WelcomeWordsDto>(dbName);
            dal.UpdateDtoToPart(data.saveData);
            return resModel;
        }

        public MuzeyResModel<ACWelcomeWordsResDto> Delete(MuzeyReqModel<ACWelcomeWordsReqDto> reqModel)
        {

            var data = reqModel.datas[0];

            var resModel = new MuzeyResModel<ACWelcomeWordsResDto>();

            string dbName = "";
            dbName = data.workShop + "※" + data.workShop + "_ANDON";
            var dal = new MuzeyBusinessLogic<ANDON_WelcomeWordsDto>(dbName);
            dal.DeleteDto(data.saveData);
            return resModel;
        }
    }
}

