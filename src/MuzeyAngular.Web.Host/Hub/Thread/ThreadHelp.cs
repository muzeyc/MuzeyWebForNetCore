using S7.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MuzeySignalr;
using Microsoft.AspNetCore.SignalR;
using System.Threading;

namespace MuzeyThread
{
    public class ThreadHelp
    {
        //超时时间(100ms)
        public static int TimeOutS = 40 * 10;
        public static log4net.ILog log = log4net.LogManager.GetLogger(typeof(ThreadHelp));
        

        public static bool ReadPoint(string point, bool b, Plc plc)
        {
            for (int i=0;i< TimeOutS; i++)
            {
                if (b)
                {
                    if (plc.Read(point).ToString() == "True")
                    {
                        return true;
                    }
                }
                else
                {
                    if (plc.Read(point).ToString() == "False")
                    {
                        return true;
                    }
                }

                Thread.Sleep(100);
            }

            return false;
        }

        public static void MuzeyLogDebug(string s, string area="WBS")
        {
            log.Debug(s);
            //MsgMQ.MqAdd(new MsgMQModel() { user = "ALL", message = string.Format("RCArea#{0}→RCLog#{1}",area,s) });
        }
    }
}
