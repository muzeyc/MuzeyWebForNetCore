using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class MuzeyReqModel<T>
    {
        public MuzeyReqModel()
        {
            this.datas = new List<T>();
        }

        public string action { get; set; }
        public int totalCount { get; set; }
        public int offset { get; set; }
        public int pageSize { get; set; }
        public string fileName { get; set; }
        public List<MuzeyColModel> cols { get; set; }
        public List<T> datas { get; set; }
    }
}
