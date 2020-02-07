using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class MuzeyResModel<T>
    {
        public MuzeyResModel()
        {
            this.datas = new List<T>();
            this.resStatus = "ok";
        }

        public void CreateErr(string msg)
        {
            this.resStatus = "err";
            this.resMsg = msg;
        }

        public string resStatus { get; set; }
        public string resMsg { get; set; }
        public int totalCount { get; set; }
        public int offset { get; set; }
        public int pageSize { get; set; }
        public List<byte> bs { get; set; }

        public List<T> datas { get; set; }
    }
}
