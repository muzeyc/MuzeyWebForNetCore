using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class BaseDataAppService
    {
        public MuzeyResModel<BaseDataResDto> GetLines(MuzeyReqModel<BaseDataReqDto> reqModel)
        {
            var filter = reqModel.datas[0];
            var resModel = new MuzeyResModel<BaseDataResDto>();
            if (string.IsNullOrEmpty(filter.workShop))
            {
                return resModel;
            }
            var dal = new MuzeyBusinessLogic<BASE_LINEDto>(filter.workShop + "※" + filter.workShop + "_ANDON");

            resModel.datas.Add(new BaseDataResDto());
            var datas = dal.GetDtoList("");
            foreach (var data in datas)
            {
                var rd = new BaseDataResDto();
                rd.text = data.LineFullName;
                rd.val = data.LineDesc;
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<BaseDataResDto> GetAlarmTypes(MuzeyReqModel<BaseDataReqDto> reqModel)
        {
            var resModel = new MuzeyResModel<BaseDataResDto>();
            var filter = reqModel.datas[0];
            if (string.IsNullOrEmpty(filter.workShop))
            {
                return resModel;
            }

            var dal = new MuzeyBusinessLogic<ALARM_ALARMTYPEDto>(filter.workShop + "※" + filter.workShop + "_ANDON");
            
            resModel.datas.Add(new BaseDataResDto() { text = "全部" });
            var datas = dal.GetDtoList("");
            foreach (var data in datas)
            {
                var rd = new BaseDataResDto();
                rd.text = data.AlarmTypeDesc;
                rd.val = string.IsNullOrEmpty(data.Remarks) ? data.AlarmTypeCode.ToString() : data.Remarks;
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<BaseDataResDto> GetDeviceTypes(MuzeyReqModel<BaseDataReqDto> reqModel)
        {
            var filter = reqModel.datas[0];
            var resModel = new MuzeyResModel<BaseDataResDto>();
            if (string.IsNullOrEmpty(filter.workShop))
            {
                return resModel;
            }
            var dal = new MuzeyBusinessLogic<ALARM_DEVICETYPEDto>(filter.workShop + "※" + filter.workShop + "_ANDON");

            resModel.datas.Add(new BaseDataResDto() { text="全部"});
            var datas = dal.GetDtoList("");
            foreach (var data in datas)
            {
                var rd = new BaseDataResDto();
                rd.text = data.DeviceTypeDesc;
                rd.val = data.DeviceTypeCode.ToString();
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        public MuzeyResModel<BaseDataResDto> GetFamilyData(MuzeyReqModel<BaseDataReqDto> reqModel)
        {
            var dal = new MuzeyBusinessLogic<AVI_FEATURES_MQDto>("AE※AE_AVI");

            var resModel = new MuzeyResModel<BaseDataResDto>();
            resModel.datas.Add(new BaseDataResDto());
            var datas = dal.GetDtoList("");
            var dic = new Dictionary<string,string>();
            foreach (var data in datas)
            {
                if (!dic.ContainsKey(data.FamilyCode))
                {
                    dic.Add(data.FamilyCode, data.FamilyName);
                }
                if (!dic.ContainsKey(data.FeatureCode))
                {
                    dic.Add(data.FeatureCode, data.FeatureName);
                }
            }

            foreach(var kv in dic)
            {
                var rd = new BaseDataResDto();
                rd.text = kv.Value;
                rd.val = kv.Key;
                resModel.datas.Add(rd);
            }

            return resModel;
        }

        public MuzeyResModel<BASE_PERSONDto> GetLogin(MuzeyReqModel<BaseDataReqDto> reqModel)
        {
            var filter = reqModel.datas[0];
            var dalLine = new MuzeyBusinessLogic<BASE_PERSONDto>("ABP_Base");

            var resModel = new MuzeyResModel<BASE_PERSONDto>();
            var datas = dalLine.GetDtoList(string.Format("AND UserId = '{0}'", filter.user));
            foreach (var data in datas)
            {
                resModel.datas.Add(data);
            }

            if(resModel.datas.Count == 0)
            {
                resModel.datas.Add(new BASE_PERSONDto() { PermissionType="0", PersonName="系统管理员"});
            }

            return resModel;
        }
    }
}
