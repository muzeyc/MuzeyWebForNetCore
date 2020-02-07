using Abp.Application.Services;
using Abp.Authorization;
using MuzeyAngular.Authorization;
using CommonUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BusinessLogic;
using Castle.Core.Logging;

namespace MuzeyServer
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class MuzeyAppService : ApplicationService, IMuzeyAppService
    {
        public MuzeyResModel<object> Req(MuzeyReqModel<object> reqModel)
        {
            var resModel = new MuzeyResModel<object>();
            try
            {
                var clazzAction = reqModel.action.Split('.');

                var reqT = typeof(MuzeyReqModel<>);
                reqT = reqT.MakeGenericType(new Type[] { AssemblyUtil.typeDic["MuzeyServer." + clazzAction[0] + "ReqDto"] });
                var reqInvokeModel = AssemblyUtil.DeSerializerModel(reqT, JsonUtil.SerializeJSON(reqModel));

                Type t = AssemblyUtil.typeDic["MuzeyServer." + clazzAction[0] + "AppService"];
                var method = t.GetMethod(clazzAction[1]);
                var obj = Activator.CreateInstance(t);
                var resObj = method.Invoke(obj, new object[] { reqInvokeModel });
                resModel = JsonUtil.DeserializeJSON<MuzeyResModel<object>>(JsonUtil.SerializeJSON(resObj));
            }
            catch (Exception ex)
            {
                resModel.resStatus = "err";
                resModel.resMsg = ex.Message;
            }

            return resModel;
        }

        //上传文件返回文件路径
        public string FileUpload(MuzeyReqModel<MuzeyUploadReqModel> reqModel)
        {
            var m = reqModel.datas[0];
            var bs = Convert.FromBase64String(m.base64);
            if (!Directory.Exists("UploadFiles"))
            {
                Directory.CreateDirectory("UploadFiles");
            }

            string path = "UploadFiles/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string resFile = path + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + m.fileName;
            var fs = new FileStream(resFile, FileMode.Create);
            fs.Write(bs,0,bs.Length);
            fs.Close();

            return resFile;
        }

        public MuzeyMenuModel GetMenuData(string user)
        {
            var resModel = new MuzeyMenuModel();
            var baseModel = new MuzeyMenuModel();

            //Home
            baseModel.childItems.Add(new MuzeyMenuModel() { name = "HomePage", icon = "home", route = "/app/home" });

            //AVI
            var avis = new List<MuzeyMenuModel>();
            avis.Add(new MuzeyMenuModel() { name = "主线计划管理", icon = "receipt", route = "/app/ac_workplanmq" });
            avis.Add(new MuzeyMenuModel() { name = "分线计划管理", icon = "settings_input_component", route = "/app/ac_lineplanmq" });
            avis.Add(new MuzeyMenuModel() { name = "计划产量", icon = "hot_tub", route = "/app/ac_planproduct" });
            avis.Add(new MuzeyMenuModel() { name = "上下线统计", icon = "swap_vert", route = "/app/ac_linese" });
            avis.Add(new MuzeyMenuModel() { name = "过点记录", icon = "group_work", route = "/app/ac_recordmq" });
            avis.Add(new MuzeyMenuModel() { name = "计划拉入拉出", icon = "transfer_within_a_station", route = "/app/ac_ordermodify" });
            avis.Add(new MuzeyMenuModel() { name = "离线车辆", icon = "offline_bolt", route = "/app/ac_offlinecar" });
            avis.Add(new MuzeyMenuModel() { name = "生产日历", icon = "date_range", route = "/app/ac_calendars" });
            avis.Add(new MuzeyMenuModel() { name = "班次休息", icon = "av_timer", route = "/app/ac_shiftrest" });
            avis.Add(new MuzeyMenuModel() { name = "冲压单批次", icon = "alternate_email", route = "/app/ac_adcsingle" });
            avis.Add(new MuzeyMenuModel() { name = "冲压订单", icon = "format_indent_increase", route = "/app/ac_seplanorder" });
            //avis.Add(new MuzeyMenuModel() { name = "过点OPC地址", icon = "nfc", route = "/app/ac_mqadressconfig" });
            baseModel.childItems.Add(new MuzeyMenuModel() { name = "AVI", icon = "shopping_cart", childItems= avis });

            //PMC
            var pmcs = new List<MuzeyMenuModel>();
            pmcs.Add(new MuzeyMenuModel() { name = "拧紧枪结果", icon = "gesture", route = "/app/ac_qcosinfo" });
            pmcs.Add(new MuzeyMenuModel() { name = "Bypass记录", icon = "nature_people", route = "/app/ac_bypass" });
            pmcs.Add(new MuzeyMenuModel() { name = "加注结果", icon = "play_for_work", route = "/app/ac_filldata" });
            pmcs.Add(new MuzeyMenuModel() { name = "拧紧枪报警", icon = "fitness_center", route = "/app/ac_warn/拧紧枪,AE,4,BE,9" });
            pmcs.Add(new MuzeyMenuModel() { name = "铆接报警", icon = "call_merge", route = "/app/ac_warn/铆接,BE,7" });
            pmcs.Add(new MuzeyMenuModel() { name = "胶量低报警", icon = "whatshot", route = "/app/ac_warn/胶量低,BE,8" });
            pmcs.Add(new MuzeyMenuModel() { name = "报警明细", icon = "warning", route = "/app/ac_warn/0" });
            pmcs.Add(new MuzeyMenuModel() { name = "停线明细", icon = "remove_shopping_cart", route = "/app/ac_stoptime" });
            pmcs.Add(new MuzeyMenuModel() { name = "冲压ADC换模记录", icon = "developer_mode", route = "/app/ac_adcstatisticsinfo" });
            pmcs.Add(new MuzeyMenuModel() { name = "冲压ADC换模统计", icon = "healing", route = "/app/ac_adcstatistics" });
            baseModel.childItems.Add(new MuzeyMenuModel() { name = "PMC", icon = "settings_input_antenna", childItems = pmcs });

            //ANDON
            var andns = new List<MuzeyMenuModel>();
            //andns.Add(new MuzeyMenuModel() { name = "物料安灯配置", icon = "filter_tilt_shift", route = "/app/ac_mcconfig" });
            andns.Add(new MuzeyMenuModel() { name = "冲压零件信息", icon = "drafts", route = "/app/ac_semouldinfo" });
            andns.Add(new MuzeyMenuModel() { name = "物料安灯记录", icon = "wb_incandescent", route = "/app/ac_materielcall" });
            baseModel.childItems.Add(new MuzeyMenuModel() { name = "ANDON", icon = "notification_important", childItems = andns });

            //配置管理
            var configs = new List<MuzeyMenuModel>();
            configs.Add(new MuzeyMenuModel() { name = "设备类型", icon = "dvr", route = "/app/ac_devicetype" });
            configs.Add(new MuzeyMenuModel() { name = "报警类型", icon = "alarm", route = "/app/ac_alarmtype" });
            configs.Add(new MuzeyMenuModel() { name = "报警系统", icon = "tune", route = "/app/ac_alarmsystem" });
            configs.Add(new MuzeyMenuModel() { name = "产线管理", icon = "linear_scale", route = "/app/ac_line" });
            configs.Add(new MuzeyMenuModel() { name = "工位管理", icon = "ev_station", route = "/app/ac_station" });
            configs.Add(new MuzeyMenuModel() { name = "邮件服务配置", icon = "mail", route = "/app/ac_mailbaseconfig" });
            configs.Add(new MuzeyMenuModel() { name = "收件人配置", icon = "contact_mail", route = "/app/ac_mailadressee" });
            configs.Add(new MuzeyMenuModel() { name = "欢迎词配置", icon = "insert_emoticon", route = "/app/ac_welcomewords" });
            baseModel.childItems.Add(new MuzeyMenuModel() { name = "配置管理", icon = "settings_applications", childItems = configs });

            //RC路由
            var rcs = new List<MuzeyMenuModel>();
            rcs.Add(new MuzeyMenuModel() { name = "队列", icon = "crop_rotate", route = "/app/ac_rcfictitious" });
            rcs.Add(new MuzeyMenuModel() { name = "规则", icon = "wrap_text", route = "/app/ac_rcrule" });
            rcs.Add(new MuzeyMenuModel() { name = "预排", icon = "list_alt", route = "/app/ac_rclog" });
            baseModel.childItems.Add(new MuzeyMenuModel() { name = "RC路由", icon = "power_input", childItems = rcs });

            //System
            var systems = new List<MuzeyMenuModel>();
            systems.Add(new MuzeyMenuModel() { name = "人员", icon = "pregnant_woman", route = "/app/ac_personinfo" });
            systems.Add(new MuzeyMenuModel() { name = "工厂", icon = "account_balance", route = "/app/ac_factoryinfo" });
            systems.Add(new MuzeyMenuModel() { name = "车间", icon = "store", route = "/app/ac_workshopinfo" });
            //systems.Add(new MuzeyMenuModel() { name = "部门", icon = "contacts", route = "/app/ac_departmentinfo" });
            systems.Add(new MuzeyMenuModel() { name = "Users", permissionName= "Pages.Users", icon = "people", route = "/app/users" });
            //systems.Add(new MuzeyMenuModel() { name = "系统测试", icon = "usb", route = "/app/ac_test" });
            //systems.Add(new MuzeyMenuModel() { name = "Roles", permissionName = "Pages.Roles", icon = "local_offer", route = "/app/roles" });
            baseModel.childItems.Add(new MuzeyMenuModel() { name = "系统管理", icon = "adb", childItems = systems });

            var dalPer = new MuzeyBusinessLogic<BASE_PERSONDto>("ABP_Base");
            var perDtos = dalPer.GetDtoList(string.Format("AND UserId='{0}'",user));
            var dalPP = new MuzeyBusinessLogic<BASE_PERSON_PERMISSIONDto>("ABP_Base");
            if(perDtos.Count == 0 || perDtos[0].PermissionType == "0")
            {
                return baseModel;
            }
            var dtoPPs = dalPP.GetDtoDic(string.Format("AND PersonID='{0}'", perDtos[0].ID),"ViewName");

            resModel.childItems.Add(baseModel.childItems[0]);

            int ii = 1;
            foreach(var cs in baseModel.childItems)
            {
                var resItems = new List<MuzeyMenuModel>();
                foreach(var ccs in cs.childItems)
                {
                    if (dtoPPs.ContainsKey(ccs.name))
                    {
                        resItems.Add(ccs);
                    }
                }
                if (resItems.Count > 0)
                {
                    resModel.childItems.Add(cs);
                    resModel.childItems[ii].childItems = resItems;
                    ii++;
                }
            }

            if (perDtos[0].PermissionType == "2" || perDtos[0].PermissionType == "4")
            {
                resModel.childItems.Add(baseModel.childItems[baseModel.childItems.Count - 2]);
            }

            if (perDtos[0].PermissionType == "3" || perDtos[0].PermissionType == "4")
            {
                resModel.childItems.Add(baseModel.childItems[baseModel.childItems.Count-1]);
                resModel.childItems[resModel.childItems.Count - 1].childItems.RemoveAt(1);
                resModel.childItems[resModel.childItems.Count - 1].childItems.RemoveAt(1);
            }

            return resModel;
        }
    }
}

