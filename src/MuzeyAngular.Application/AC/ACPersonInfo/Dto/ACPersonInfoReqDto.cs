using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACPersonInfoReqDto
    {

        public ACPersonInfoReqDto()
        {

        }

        public string id { get; set; }
        [MuzeyReqType]
        public string workShop { get; set; }
        [MuzeyReqType]
        public string personName { get; set; }
        public BASE_PERSONDto saveData { get; set; }
        public MuzeyMenuModel Views { get; set; }
    }
}
