using Abp.AspNetCore.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using MuzeyThread;
using S7.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MuzeySignalr
{

    public class MuzeySignalrCommon : Hub
    {

        public void SendMessage(string user, string message)
        {
            Clients.All.SendAsync("MuzeySignalr", user, message);
        }

        public void ChangeMode(string area, string message)
        {
            ThreadHelp.log.Debug("ģʽ�л�->" + "����->" + area + "message->" + message);
            if(area == "PBS")
            {
                Plc plcPbs = new Plc(CpuType.S71500, "10.25.96.12", 0, 1);
                plcPbs.Open();
                if(message == "�ӹ�ģʽ")
                {
                    plcPbs.Write("DB2017.DBX76.2", false);
                }
                else
                {
                    plcPbs.Write("DB2017.DBX76.2", true);
                }
                plcPbs.Close();
            }
            
            if (area == "WBS")
            {
                Plc plcWbs = new Plc(CpuType.S71500, "10.25.248.72", 0, 1);
                plcWbs.Open();
                if (message == "�ӹ�ģʽ")
                {
                    plcWbs.Write("DB39000.DBX86.3", false);
                }
                else
                {
                    plcWbs.Write("DB39000.DBX86.3", true);
                }
                plcWbs.Close();
            }
        }

        public void Register()
        {
        }
    }
}