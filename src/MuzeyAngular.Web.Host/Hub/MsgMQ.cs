using Abp.AspNetCore.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using MuzeyThread;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MuzeySignalr
{

    public static class MsgMQ
    {
        static MsgMQ()
        {
            msgList = new List<MsgMQModel>();
        }

        public static List<MsgMQModel> msgList { get; set; }
        public static void MqAdd(MsgMQModel m)
        {
            lock (msgList)
            {
                msgList.Add(m);
            }
        }

        public static void MqRemove()
        {
            if (msgList.Count > 0)
            {
                msgList.RemoveAt(0);
            }
        }
    }

    public class MsgMQModel
    {
        public string user { get; set; }
        public string message { get; set; }
    }
}