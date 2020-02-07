using S7.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MuzeySignalr;
using Microsoft.AspNetCore.SignalR;
using BusinessLogic;
using System.Data;
using MuzeyRC;
using MuzeyServer;

namespace MuzeyThread
{
    public class ThreadSub
    {
        public Plc plc;
        public MuzeyRCHelp mRcH;
        public ThreadSub(Plc plc)
        {
            this.plc = plc;
            mRcH = new MuzeyRCHelp();
        }


        public void SendLog(string user, string message)
        {
            MsgMQ.MqAdd(new MsgMQModel() { user = user, message = message });
        }

        public void TEST()
        {
            //SendLog("触发事件", plc.Read("DB3.DBW1076").ToString());
            //plc.Write("DB1010.DBX6540.1", true);
            //if(ThreadHelp.ReadPoint("DB1010.DBX6540.0", false, plc))
            //{
            //    SendLog("触发事件完成", "很好很强大");
            //}
            //else
            //{
            //    SendLog("触发事件超时", "和伤心");
            //}
        }

        public void PBSINROAD()
        {
            ThreadHelp.MuzeyLogDebug("PBS入道流程开始","PBS");
            var vin = plc.Read(DataType.DataBlock, 2017, 28, VarType.String, 17).ToString();//VIN 长度17位
            ThreadHelp.MuzeyLogDebug("读取VIN->" + vin, "PBS");
            //验证VIN是否合法
            if (!(vin[0] >= 33 && vin[0] <= 126))
            {
                ThreadHelp.MuzeyLogDebug("VIN不合法终止流程->" + vin, "PBS");
                return;
            }

            //通过vin码得到进入的道 加入车道队列
            short inNum;
            var res = mRcH.InGetRCRoad("PBS", vin);
            if (res.state)
            {
                inNum = res.roadNum;
                ThreadHelp.MuzeyLogDebug("计算入道号为->" + inNum, "PBS");
            }
            else
            {
                ThreadHelp.MuzeyLogDebug(res.msg, "PBS");
                return;
            }
            
            plc.Write("DB2017.DBW48", inNum);//1、2、3、4(快速道)、5(返回道-不能发)、6、7、8、9、10
            ThreadHelp.MuzeyLogDebug("写入道号->" + inNum, "PBS");
            plc.Write("DB2017.DBX46.0", true);//写入完成信号
            ThreadHelp.MuzeyLogDebug("写入完成信号入道流程结束", "PBS");
        }

        public void PBSINCAR()
        {
            ThreadHelp.MuzeyLogDebug("PBS入车流程开始", "PBS");
            var vin = plc.Read(DataType.DataBlock, 2017, 28, VarType.String, 17).ToString();//VIN 长度17位
            ThreadHelp.MuzeyLogDebug("读取VIN->" + vin, "PBS");
            //验证VIN是否合法
            if (!(vin[0] >= 33 && vin[0] <= 126))
            {
                ThreadHelp.MuzeyLogDebug("VIN不合法终止流程->" + vin, "PBS");
                plc.Write("DB2017.DBX46.0", false);
                plc.Write("DB2017.DBW48", (short)0);//地址清除
                return;
            }
            var inNum = plc.Read("DB2017.DBW48").ToString();
            //验证道号是否合法
            ThreadHelp.MuzeyLogDebug("读取道号->" + inNum, "PBS");
            if (inNum == "0")
            {
                ThreadHelp.MuzeyLogDebug("道号不合法终止流程->" + inNum, "PBS");
                plc.Write("DB2017.DBX46.0", false);
                plc.Write("DB2017.DBW48", (short)0);//地址清除
                return;
            }

            //插入操作日志
            var dalOp = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            dalOp.InsertDto(new RC_InOutLogDto()
            {
                Area = "PBS"
                , Road = inNum
                , VIN = vin
                , State = "1"
                , OpTime = DateTime.Now.ToString()
            });
            ThreadHelp.MuzeyLogDebug("操作日志插入完成", "PBS");

            //更新队列
            var resMsg = RCCommon.RCUpdateRoadIn("PBS", inNum, vin);
            if (resMsg != "0")
            {
                ThreadHelp.MuzeyLogDebug("队列更新错误->" + resMsg, "PBS");
            }
            else
            {
                ThreadHelp.MuzeyLogDebug("队列更新完成", "PBS");
            }

            plc.Write("DB2017.DBW48", (short)0);//地址清除
            plc.Write("DB2017.DBX46.0", false);//地址清除
            ThreadHelp.MuzeyLogDebug("PBS入车流程结束", "PBS");
            SendLog("ALL", "RCArea#PBS→RCReset#1");
        }

        public void PBSOUTROAD()
        {
            //判断该车道上出车道是否有车
            //plc.Read("DB2017.DBX18.0");1
            //plc.Read("DB2017.DBX18.1");2
            //plc.Read("DB2017.DBX18.2");3
            //plc.Read("DB2017.DBX18.3");4
            //plc.Read("DB2017.DBX18.4");5
            //plc.Read("DB2017.DBX18.5");6
            //plc.Read("DB2017.DBX18.6");7
            //plc.Read("DB2017.DBX18.7");8
            //plc.Read("DB2017.DBX19.0");9
            //plc.Read("DB2017.DBX19.1");10
            ThreadHelp.MuzeyLogDebug("PBS出道流程开始", "PBS");
            //根据算法得出需要出去的道
            var res = mRcH.OutGetRCRoad("PBS");
            short outNum = 0;
            if (res.state)
            {
                outNum = res.roadNum;
                ThreadHelp.MuzeyLogDebug("计算出道号为->" + outNum, "PBS");
                var address = "";
                if (outNum <= 8)
                {
                    address = "DB2017.DBX18." + (outNum - 1).ToString();
                }
                else if (outNum > 8)
                {
                    address = "DB2017.DBX19." + (outNum - 9).ToString();
                }

                if (plc.Read(address).ToString() != "True")
                {
                    ThreadHelp.MuzeyLogDebug("车未到位流程终止", "PBS");
                    return;
                }
            }
            else
            {
                ThreadHelp.MuzeyLogDebug(res.msg, "PBS");
                return;
            }

            plc.Write("DB2017.DBW78", outNum);//出道号1、2、3、4(快速道)、5(返回道-不能发)、6、7、8、9、10
            //根据情况判断是出道还是返回
            short type = short.Parse(res.isRev ? "2" : "1");
            plc.Write("DB2017.DBW80", type);//1:出车 2:返回道
            plc.Write("DB2017.DBX76.0", true);//写完成信息号
            ThreadHelp.MuzeyLogDebug("道号->" + outNum + " 类型->" + type, "PBS");
            ThreadHelp.MuzeyLogDebug("PBS出道流程结束", "PBS");
        }

        public void PBSOUTCAR()
        {
            ThreadHelp.MuzeyLogDebug("PBS出车流程开始", "PBS");
            string vin = plc.Read(DataType.DataBlock, 2017, 58, VarType.String, 17).ToString();//VIN 长度17位
            ThreadHelp.MuzeyLogDebug("读取VIN->" + vin, "PBS");
            //验证VIN是否合法
            if (!(vin[0] >= 33 && vin[0] <= 126))
            {
                ThreadHelp.MuzeyLogDebug("VIN不合法终止流程->" + vin, "PBS");
                return;
            }
            var outNum = plc.Read("DB2017.DBW78").ToString();//出道号1、2、3、4(快速道)、5(返回道-不能发)、6、7、8、9、10
            ThreadHelp.MuzeyLogDebug("读取道号->" + outNum, "PBS");
            if (outNum == "0")
            {
                ThreadHelp.MuzeyLogDebug("道号不合法终止流程->" + outNum, "PBS");
                return;
            }

            //插入操作日志
            var dalOp = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            dalOp.InsertDto(new RC_InOutLogDto()
            {
                Area = "PBS"
                ,
                Road = outNum
                ,
                VIN = vin
                ,
                State = "2"
                ,
                OpTime = DateTime.Now.ToString()
            });
            ThreadHelp.MuzeyLogDebug("操作日志插入完成", "PBS");

            //判断当前数据库VIN码与PLC读出的VIN码是否一致不一致则报警 更新队列数据库
            //更新队列
            var resMsg = RCCommon.RCUpdateRoadOut("PBS", outNum, vin);
            if (resMsg != "0")
            {
                ThreadHelp.MuzeyLogDebug(resMsg, "PBS");
            }
            else
            {
                ThreadHelp.MuzeyLogDebug("队列更新完成", "PBS");
                plc.Write("DB2017.DBX76.1", true);//放行信号
            }
            ThreadHelp.MuzeyLogDebug("PBS出车流程结束", "PBS");
            SendLog("ALL", "RCArea#PBS→RCReset#1");
        }

        public void PBSOUTCAREEND()
        {
            plc.Write("DB2017.DBW78", (short)0);
            plc.Write("DB2017.DBX76.0", false);
            plc.Write("DB2017.DBW80", (short)0);
            plc.Write("DB2017.DBX76.1", false);
            ThreadHelp.MuzeyLogDebug("PBS出车完成流程结束", "PBS");
            SendLog("ALL", "RCArea#WBS→RCReset#1");
        }

        public void WBSINROAD()
        {
            ThreadHelp.MuzeyLogDebug("WBS入道流程开始");
            string vin = plc.Read(DataType.DataBlock, 39000, 36, VarType.String, 17).ToString();//VIN 长度17位
            //通过vin码得到进入的道
            ThreadHelp.MuzeyLogDebug("读取VIN->" + vin);
            //验证VIN是否合法
            if (!(vin[0] >= 33 && vin[0] <= 126))
            {
                ThreadHelp.MuzeyLogDebug("VIN不合法终止流程->" + vin);
                return;
            }
            short inNum;
            var res = mRcH.InGetRCRoad("WBS", vin);
            if (res.state)
            {
                inNum = res.roadNum;
                ThreadHelp.MuzeyLogDebug("计算入道号为->" + inNum);
            }
            else
            {
                ThreadHelp.MuzeyLogDebug(res.msg);
                return;
            }
            plc.Write("DB39000.DBW56", inNum);//1、2、3、4
            ThreadHelp.MuzeyLogDebug("写入道号->" + inNum);
            plc.Write("DB39000.DBX58.0", true);//写入完成信号
            ThreadHelp.MuzeyLogDebug("写入完成信号入道流程结束");
        }

        public void WBSINCAR()
        {
            ThreadHelp.MuzeyLogDebug("WBS入车流程开始");
            string vin = plc.Read(DataType.DataBlock, 39000, 36, VarType.String, 17).ToString();//VIN 长度17位
            ThreadHelp.MuzeyLogDebug("读取VIN->" + vin);
            //验证VIN是否合法
            if (!(vin[0] >= 33 && vin[0] <= 126))
            {
                ThreadHelp.MuzeyLogDebug("VIN不合法终止流程->" + vin);
                plc.Write("DB39000.DBX58.0", false);//写入完成信号
                plc.Write("DB39000.DBW56", (short)0);//地址清除
                return;
            }

            var inNum = plc.Read("DB39000.DBW56").ToString();
            //验证道号是否合法
            ThreadHelp.MuzeyLogDebug("读取道号->" + inNum);
            if (inNum == "0")
            {
                ThreadHelp.MuzeyLogDebug("道号不合法终止流程->" + inNum);
                plc.Write("DB39000.DBX58.0", false);//写入完成信号
                plc.Write("DB39000.DBW56", (short)0);//地址清除
                return;
            }

            //插入操作日志
            var dalOp = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            dalOp.InsertDto(new RC_InOutLogDto()
            {
                Area = "WBS"
                ,
                Road = inNum
                ,
                VIN = vin
                ,
                State = "1"
                ,
                OpTime = DateTime.Now.ToString()
            });
            ThreadHelp.MuzeyLogDebug("操作日志插入完成");

            //更新队列
            var resMsg = RCCommon.RCUpdateRoadIn("WBS", inNum, vin);
            if (resMsg != "0")
            {
                ThreadHelp.MuzeyLogDebug("队列更新错误->" + resMsg);
            }
            else
            {
                ThreadHelp.MuzeyLogDebug("队列更新完成");
            }

            plc.Write("DB39000.DBW56", (short)0);//地址清除
            plc.Write("DB39000.DBX58.0", false);//地址清除
            ThreadHelp.MuzeyLogDebug("WBS入车流程结束");
            SendLog("ALL", "RCArea#WBS→RCReset#1");
        }

        public void WBSOUTROAD()
        {
            //判断该车道上出车道是否有车
            //plc.Read("DB39000.DBX0.1");
            //plc.Read("DB39000.DBX0.2");
            //plc.Read("DB39000.DBX0.3");
            //plc.Read("DB39000.DBX0.4");
            ThreadHelp.MuzeyLogDebug("WBS出道流程开始");
            //根据算法得出需要出去的道
            var res = mRcH.OutGetRCRoad("WBS");
            short outNum = 0;
            if (res.state)
            {
                outNum = res.roadNum;
                ThreadHelp.MuzeyLogDebug("计算出道号为->" + outNum);
                var address = "DB39000.DBX0." + res.roadNum;
                if (plc.Read(address).ToString() != "True")
                {
                    ThreadHelp.MuzeyLogDebug("车未到位流程终止");
                    return;
                }
            }
            else
            {
                ThreadHelp.MuzeyLogDebug(res.msg);
                return;
            }

            plc.Write("DB39000.DBW60", outNum);//出道号1、2、3、4
            //判断出车还是返回
            short type = short.Parse(res.isRev ? "2" : "1");
            plc.Write("DB39000.DBW62", type);//1:出车 2:返回道
            plc.Write("DB39000.DBX64.0", true);//写完成信息号
            ThreadHelp.MuzeyLogDebug("道号->" + outNum + " 类型->" + type);
            if(type == 2)
            {
                //如果走返回道--升降机车道占用中拒绝操作
                if (ThreadHelp.ReadPoint("DB39000.DBX86.2", true, plc))
                {
                    ThreadHelp.MuzeyLogDebug("升降机占用中,流程终止,信号清除！");
                    plc.Write("DB39000.DBW60", (short)0);//出道号1、2、3、4、5(返回道+快速道-不能发)
                    plc.Write("DB39000.DBW62", (short)0);//1:出车 2:返回道
                    plc.Write("DB39000.DBX64.0", false);//写完成信息号
                }
            }
            ThreadHelp.MuzeyLogDebug("WBS出道流程结束");
        }

        public void WBSOUTCAR()
        {
            ThreadHelp.MuzeyLogDebug("WBS出车流程开始");
            var vin = plc.Read(DataType.DataBlock, 39000, 68, VarType.String, 17).ToString();//VIN 长度17位
            ThreadHelp.MuzeyLogDebug("读取VIN->" + vin);
            //验证VIN是否合法
            if (!(vin[0] >= 33 && vin[0] <= 126))
            {
                ThreadHelp.MuzeyLogDebug("VIN不合法终止流程->" + vin);
                plc.Write("DB39000.DBX64.0", false);//写完成信息号
                return;
            }

            var outNum = plc.Read("DB39000.DBW60").ToString();
            ThreadHelp.MuzeyLogDebug("读取道号->" + outNum);
            if (outNum == "0")
            {
                ThreadHelp.MuzeyLogDebug("道号不合法终止流程->" + outNum);
                plc.Write("DB39000.DBX64.0", false);//写完成信息号
                return;
            }

            //插入操作日志
            var dalOp = new MuzeyBusinessLogic<RC_InOutLogDto>("ABP_Base");
            dalOp.InsertDto(new RC_InOutLogDto()
            {
                Area = "WBS"
                ,
                Road = outNum
                ,
                VIN = vin
                ,
                State = "2"
                ,
                OpTime = DateTime.Now.ToString()
            });
            ThreadHelp.MuzeyLogDebug("操作日志插入完成");

            //判断当前数据库VIN码与PLC读出的VIN码是否一致不一致则报警 更新队列数据库
            //更新队列
            var resMsg = RCCommon.RCUpdateRoadOut("WBS", outNum, vin);
            if (resMsg != "0")
            {
                ThreadHelp.MuzeyLogDebug(resMsg);
            }
            else
            {
                ThreadHelp.MuzeyLogDebug("队列更新完成");
                plc.Write("DB39000.DBX64.1", true);//放行信号
                if (ThreadHelp.ReadPoint("DB39000.DBX86.1", true, plc))
                {
                    ThreadHelp.MuzeyLogDebug("出车成功");
                }
                else
                {
                    ThreadHelp.MuzeyLogDebug("出车失败");
                }
            }

            plc.Write("DB39000.DBW60", (short)0);
            plc.Write("DB39000.DBX64.0", false);
            plc.Write("DB39000.DBW62", (short)0);
            plc.Write("DB39000.DBX64.1", false);
            ThreadHelp.MuzeyLogDebug("WBS出车流程结束");
            SendLog("ALL", "RCArea#WBS→RCReset#1");
        }

        public void WBSLIFT()
        {
            ThreadHelp.MuzeyLogDebug("升降机流程开始");
            var vin = plc.Read(DataType.DataBlock, 39000, 4, VarType.String, 17).ToString();//VIN 长度17位
            var orderType = plc.Read(DataType.DataBlock, 39000, 24, VarType.String, 4).ToString();//订单类型
            ThreadHelp.MuzeyLogDebug("VIN->" + vin + "->orderType->" + orderType);

            //验证VIN是否合法
            if (!(vin[0] >= 33 && vin[0] <= 126))
            {
                ThreadHelp.MuzeyLogDebug("VIN不合法终止流程->" + vin);
                return;
            }

            //写方向1:入库区 2:快速道
            var res = new RuleCore().ISUP(new RC_RuleDto() { Road = "01" }, "WBS", vin, "");
            //写方向1:入库区 2:快速道
            short type = short.Parse(res.isSub ? "2" : "1");
            ThreadHelp.MuzeyLogDebug("走" + (res.isSub ? "快速道" : "入库区"));
            plc.Write("DB39000.DBW30", type);
            plc.Write("DB39000.DBX32.0", true);//写入完成信号
            if (!ThreadHelp.ReadPoint("DB39000.DBX28.1", true, plc))
            {
                ThreadHelp.MuzeyLogDebug("超时未收到完成信号DB39000.DBX28.1");
            }

            //清除地址
            plc.Write("DB39000.DBW30", (short)0);
            plc.Write("DB39000.DBX32.0", false);//写入完成信号
            ThreadHelp.MuzeyLogDebug("升降机流程结束");
        }
    }
}
