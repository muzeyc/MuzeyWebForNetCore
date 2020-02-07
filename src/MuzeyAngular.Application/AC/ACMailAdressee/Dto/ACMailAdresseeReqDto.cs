using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACMailAdresseeReqDto
    {
        public string id { get; set; }
        [MuzeyReqType]
        public string workShop { get; set; }
        [MuzeyReqType]
        public string adressee { get; set; }
        public MAIL_ADRESSEEDto saveData { get; set; }
    }
}
