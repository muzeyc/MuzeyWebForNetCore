using BusinessLogic;
using System.Collections.Generic;

namespace MuzeyServer
{
    public class ACPersonInfoResDto : BASE_PERSONDto
    {
        public ACPersonInfoResDto()
        {
            this.fOps = new List<MuzeySelectModel>();
            this.wsOps = new List<MuzeySelectModel>();
            this.dOps = new List<MuzeySelectModel>();
            this.aOps = new List<MuzeySelectModel>();
        }

        public List<MuzeySelectModel> fOps { get; set; }
        public List<MuzeySelectModel> wsOps { get; set; }
        public List<MuzeySelectModel> dOps { get; set; }
        public List<MuzeySelectModel> aOps { get; set; }
        public MuzeyMenuModel Views { get; set; }
    }
}
