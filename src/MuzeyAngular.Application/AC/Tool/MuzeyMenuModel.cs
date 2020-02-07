using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class MuzeyMenuModel
    {
        public MuzeyMenuModel()
        {
            this.childItems = new List<MuzeyMenuModel>();
        }

        public string name { get; set; }
        public string permissionName { get; set; }
        public string icon { get; set; }
        public string route { get; set; }
        public string selected { get; set; }
        public List<MuzeyMenuModel> childItems { get; set; }
    }
}
