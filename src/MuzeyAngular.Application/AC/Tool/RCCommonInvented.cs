using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CommonUtils;
using System.Transactions;

namespace MuzeyServer
{
    public class RCCommonInvented
    {
        /// <summary>
        /// 更新队列(进)
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="Road"></param>
        /// <param name="vin"></param>
        /// <returns></returns>
        public static string RCUpdateRoadIn(string Area, string Road, string vin)
        {
            try
            {
                var dto = SqlHelp.Query(string.Format("◎ABP_Base◎SELECT TOP 1 * FROM RC_CacheInvented WHERE Area='{0}' and (vin is null or vin='') AND Road LIKE '%{1}'", Area, Road)).Tables[0].DataTableToList<RC_CacheDto>()[0];
                dto.VIN = vin;
                if (!string.IsNullOrEmpty(vin))
                {
                    var maxSeq = SqlHelp.Query(string.Format("◎ABP_Base◎SELECT TOP 1 ISNULL(max(Seq), 0) as seq from RC_CacheInvented where Area = '{0}'", Area)).Tables[0].Rows[0][0].ToInt();
                    dto.Seq = maxSeq + 1;
                }

                dto.State = "0";

                var dal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
                dal.ChangeTableName("RC_CacheInvented");
                //确认是否预冻结车辆
                var opDal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
                var dDtos = opDal.GetDtoList(string.Format("AND Area='{0}' AND VIN='{1}' AND State='7'", Area, vin));
                //该VIN为预冻结
                if (dDtos.Count > 0)
                {
                    dto.State = "1";
                    //删除预冻结
                    opDal.DeleteDtoList(dDtos);
                }
                //确认是否为冻结车返回重进
                var dcDtos = opDal.GetDtoList(string.Format("AND Area='{0}' AND VIN='{1}' AND State='3'", Area, vin));
                //该VIN为冻结车
                if (dcDtos.Count > 0)
                {
                    dto.State = "1";
                    // 删除冻结记录
                    opDal.DeleteDtoList(dcDtos);
                }
                dal.UpdateDtoToPart(dto);
                return "0";
            }
            catch(Exception e)
            {
                return "队列更新失败->" + e.Message;
            }
        }

        /// <summary>
        /// 更新队列(出)
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="Road"></param>
        /// <param name="vin"></param>
        /// <returns></returns>
        public static string RCUpdateRoadOut(string Area, string Road, string vin)
        {
            try
            {
                var dal = new MuzeyBusinessLogic<RC_CacheDto>("ABP_Base");
                dal.ChangeTableName("RC_CacheInvented");
                var dtos = dal.GetDtoList(string.Format("AND Area='{0}' AND Road like '%{1}' order by CacheCode", Area, Road));
                if (vin != dtos[0].VIN)
                {
                    var errMsg = "队列更新失败->区域->" + Area + "->车道->" + Road + "->VIN->" + vin + "与队列VIN->" + dtos[0].VIN + "->不匹配";
                    return errMsg;
                }

                //判断该车是否为冻结车
                if (dtos[0].State == "1")
                {
                    var opDal = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
                    opDal.InsertDto(new RC_InOutLogDto() { Area = Area, VIN = vin, State = "3", OpTime = DateTime.Now.ToString() });
                }

                dtos[0].State = "0";
                dtos[0].Seq = 0;
                for (int i = 0; i < dtos.Count; i++)
                {
                    if ((i + 1) == dtos.Count)
                    {
                        dtos[i].VIN = "";
                        dtos[i].Seq = 0;
                        dtos[i].State = "0";
                    }
                    else
                    {
                        dtos[i].VIN = dtos[i + 1].VIN;
                        dtos[i].Seq = dtos[i + 1].Seq;
                        dtos[i].State = dtos[i + 1].State;
                    }
                }

                dal.UpdateDtoListToPart(dtos);
                return "0";
            }
            catch (Exception e)
            {
                return "队列更新失败->" + e.Message;
            }
        }
    }
}
