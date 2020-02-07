using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACDepartmentInfoReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string departmentName { get; set; }
        public BASE_DEPARTMENTDto saveData { get; set; }
    }
}
