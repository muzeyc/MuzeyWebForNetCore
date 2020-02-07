using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACWelcomeWordsReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string chinese { get; set; }
        public ANDON_WelcomeWordsDto saveData { get; set; }
    }
}
