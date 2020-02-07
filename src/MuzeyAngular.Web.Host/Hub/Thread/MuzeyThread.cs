using S7.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using MuzeySignalr;
using Microsoft.AspNetCore.SignalR;

namespace MuzeyThread
{
    public class MuzeyThreadManager
    {
        public Dictionary<string, PlcThreadSub> ThreadMDic;

        public MuzeyThreadManager()
        {
            ThreadHelp.log.Debug("RC路由服务开启！");
            ThreadMDic = new Dictionary<string, PlcThreadSub>();
            //测试
            Plc plc = new Plc(CpuType.S71500, "10.25.248.21", 0, 1);
            PlcThreadSub testSub = new PlcThreadSub("DB3100.DBX4.2", "TEST", plc);
            var testThread = new Thread(testSub.SubMain);
            testThread.IsBackground = true;
            ThreadMDic.Add("TEST", testSub);

            //PBS
            Plc plcPBSInR = new Plc(CpuType.S71500, "10.25.96.12", 0, 1);
            Plc plcPBSInC = new Plc(CpuType.S71500, "10.25.96.12", 0, 1);
            Plc plcPBSOutR = new Plc(CpuType.S71500, "10.25.96.12", 0, 1);
            Plc plcPBSOutC = new Plc(CpuType.S71500, "10.25.96.12", 0, 1);

            //PBSROAD入道流程
            PlcThreadSub pbsInRoadSub = new PlcThreadSub("DB2017.DBX24.0", "PBSINROAD", plcPBSInR);
            var pbsInRoadThread = new Thread(pbsInRoadSub.SubMain);
            pbsInRoadThread.IsBackground = true;
            pbsInRoadThread.Start();
            ThreadMDic.Add("PBSINROAD", pbsInRoadSub);

            //PBSCAR入车流程
            PlcThreadSub pbsInCarSub = new PlcThreadSub("DB2017.DBX24.1", "PBSINCAR", plcPBSInC);
            var pbsInCarThread = new Thread(pbsInCarSub.SubMain);
            pbsInCarThread.IsBackground = true;
            pbsInCarThread.Start();
            ThreadMDic.Add("PBSINCAR", pbsInCarSub);

            //PBS出道流程
            PlcThreadSub pbsOutRoadSub = new PlcThreadSub("DB2017.DBX54.0", "PBSOUTROAD", plcPBSOutR);
            var pbsOutRoadThread = new Thread(pbsOutRoadSub.SubMain);
            pbsOutRoadThread.IsBackground = true;
            pbsOutRoadThread.Start();
            ThreadMDic.Add("PBSOUTROAD", pbsOutRoadSub);

            //PBS出车流程
            PlcThreadSub pbsOutCarSub = new PlcThreadSub("DB2017.DBX54.1", "PBSOUTCAR", plcPBSOutC);
            var pbsOutCarThread = new Thread(pbsOutCarSub.SubMain);
            pbsOutCarThread.IsBackground = true;
            pbsOutCarThread.Start();
            ThreadMDic.Add("PBSOUTCAR", pbsOutCarSub);

            //PBS出车完成流程
            PlcThreadSub pbsOutCarEndSub = new PlcThreadSub("DB2017.DBX54.2", "PBSOUTCAREEND", plcPBSOutC);
            var pbsOutCarEndThread = new Thread(pbsOutCarEndSub.SubMain);
            pbsOutCarEndThread.IsBackground = true;
            pbsOutCarEndThread.Start();
            ThreadMDic.Add("PBSOUTCAREEND", pbsOutCarEndSub);

            //WBS
            Plc plcWBSInR = new Plc(CpuType.S71500, "10.25.248.72", 0, 1);
            Plc plcWBSInC = new Plc(CpuType.S71500, "10.25.248.72", 0, 1);
            Plc plcWBSOutR = new Plc(CpuType.S71500, "10.25.248.72", 0, 1);
            Plc plcWBSOutC = new Plc(CpuType.S71500, "10.25.248.72", 0, 1);
            Plc plcWBSLift = new Plc(CpuType.S71500, "10.25.248.72", 0, 1);

            //WBS入道流程
            PlcThreadSub wbsInRoadSub = new PlcThreadSub("DB39000.DBX54.0", "WBSINROAD", plcWBSInR);
            var wbsInRoadThread = new Thread(wbsInRoadSub.SubMain);
            wbsInRoadThread.IsBackground = true;
            wbsInRoadThread.Start();
            ThreadMDic.Add("WBSINROAD", wbsInRoadSub);

            //WBS入车流程
            PlcThreadSub wbsInCarSub = new PlcThreadSub("DB39000.DBX54.1", "WBSINCAR", plcWBSInC);
            var wbsInCarThread = new Thread(wbsInCarSub.SubMain);
            wbsInCarThread.IsBackground = true;
            wbsInCarThread.Start();
            ThreadMDic.Add("WBSINCAR", wbsInCarSub);

            //WBS出道流程
            PlcThreadSub wbsOutRoadSub = new PlcThreadSub("DB39000.DBX64.2", "WBSOUTROAD", plcWBSOutR);
            var wbsOutRoadThread = new Thread(wbsOutRoadSub.SubMain);
            wbsOutRoadThread.IsBackground = true;
            wbsOutRoadThread.Start();
            ThreadMDic.Add("WBSOUTROAD", wbsOutRoadSub);

            //WBS出车流程
            PlcThreadSub wbsOutCarSub = new PlcThreadSub("DB39000.DBX86.0", "WBSOUTCAR", plcWBSOutC);
            var wbsOutCarThread = new Thread(wbsOutCarSub.SubMain);
            wbsOutCarThread.IsBackground = true;
            wbsOutCarThread.Start();
            ThreadMDic.Add("WBSOUTCAR", wbsOutCarSub);

            //升降台
            //WBS出车流程
            PlcThreadSub wbsLiftSub = new PlcThreadSub("DB39000.DBX28.0", "WBSLIFT", plcWBSLift);
            var wbsLiftThread = new Thread(wbsLiftSub.SubMain);
            wbsLiftThread.IsBackground = true;
            wbsLiftThread.Start();
            ThreadMDic.Add("WBSLIFT", wbsLiftSub);
        }

        public void ThreadStart(List<string> tNames)
        {
            foreach (var n in tNames)
            {
                if (ThreadMDic.ContainsKey(n))
                {
                    ThreadMDic[n].doFlag = true;
                    ThreadHelp.log.Debug("线程开启->" + n);
                }
            }
        }

        public void ThreadClose(List<string> tNames)
        {
            foreach (var n in tNames)
            {
                if (ThreadMDic.ContainsKey(n))
                {
                    ThreadMDic[n].doFlag = false;
                    ThreadHelp.log.Debug("线程关闭->" + n);
                }
            }
        }
    }

    public class PlcThreadSub
    {
        public void SendLog(string user, string message)
        {
            MsgMQ.MqAdd(new MsgMQModel() { user = user, message = message });
        }

        public string readP { get; set; }
        public string endP { get; set; }
        public string sub { get; set; }
        public bool doFlag { get; set; }
        public Plc S7Plc;
        public ThreadSub subObj;
        public MethodInfo subM;
        public object[] paramObjs;
        public string curReadFlag{ get; set;}

        public PlcThreadSub()
        {
            doFlag = true;
        }

        public PlcThreadSub(string readP, string sub, Plc S7Plc)
        {
            ThreadHelp.log.Debug("线程初始化->" + sub);
            this.readP = readP;
            this.sub = sub;
            doFlag = false;
            this.S7Plc = S7Plc;
            subM = typeof(ThreadSub).GetMethod(sub);
            subObj = new ThreadSub(this.S7Plc);
            curReadFlag = "False";
        }

        public void SubMain()
        {
            while (true)
            {
                while (doFlag)
                {
                    try
                    {
                        //确认PLC连接性
                        if (!S7Plc.IsConnected)
                        {
                            SendLog("ALL", string.Format("RCArea#{0}→RCConIp#{1};RCConState#0;RCCurReadFlag#{2}", BusinessLogic.ConnectionInfo.GetRCInfo(S7Plc.IP), S7Plc.IP, curReadFlag == "True" ? "1" : "0"));
                            var conResCode = S7Plc.Open();
                            if (conResCode == ErrorCode.NoError)
                            {
                                ThreadHelp.log.Debug("连接Plc->" + S7Plc.IP + "成功！");
                            }
                            else
                            {
                                ThreadHelp.log.Debug("连接Plc->" + S7Plc.IP + "失败！-->" + conResCode.ToString());
                            }

                            curReadFlag = "False";
                        }
                        else
                        {
                            SendLog("ALL", string.Format("RCArea#{0}→RCConIp#{1};RCConState#1;RCCurReadFlag#{2}", BusinessLogic.ConnectionInfo.GetRCInfo(S7Plc.IP), S7Plc.IP, curReadFlag == "True" ? "1" : "0"));
                            //如果需求点为True则执行对应方法
                            var readStr = S7Plc.Read(readP).ToString();
                            if (readStr == "True")
                            {
                                if (curReadFlag == "False")
                                {
                                    curReadFlag = "True";
                                    ThreadHelp.log.Debug(readP + "->触发");
                                    subM.Invoke(subObj, paramObjs);
                                    ThreadHelp.log.Debug(sub + "->方法触发完成");
                                }
                            }
                            else if (readStr == "False")
                            {
                                curReadFlag = "False";
                            }
                            else
                            {
                                ThreadHelp.log.Debug("触发点:" + readP + "读取错误->" + readStr);
                                curReadFlag = "False";
                            }
                        }

                        Thread.Sleep(3333);
                    }
                    catch (Exception e)
                    {
                        ThreadHelp.log.Debug(e.Message);
                        ThreadHelp.log.Debug(e.StackTrace);
                    }
                }

                Thread.Sleep(3333);
            }
        }
    }
}
