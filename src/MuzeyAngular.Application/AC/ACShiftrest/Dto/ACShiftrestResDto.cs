using BusinessLogic;

namespace MuzeyServer
{
    public class ACShiftrestResDto : AVI_SHIFTRESTDto
    {
        public string crossDayName { get; set; }
        public string workDay { get; set; }
        public string sT { get; set; }
        public string eT { get; set; }
    }
}
