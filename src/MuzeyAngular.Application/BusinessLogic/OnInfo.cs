using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class OnInfo
    {
        public string f1;
        public string f2;
        public string judgeStr;
        public string je;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <param name="je"></param>
        /// <param name="judgeStr"></param>
        public OnInfo(object o1, object o2, JoinEnum je=JoinEnum.INNER, string judgeStr = "=")
        {
            string s = o1.GetType().FullName.Split('.')[1].Split('+')[0];
            this.f1 = s.Substring(0, s.Length - 3) + "∷" + o1.ToString();
            s = o2.GetType().FullName.Split('.')[1].Split('+')[0];
            this.f2 = s.Substring(0, s.Length - 3) + "∷" + o2.ToString();
            this.judgeStr = judgeStr;
            this.je = " " + je.ToString() + " JOIN ";
        }

        public OnInfo(object o1, Type t, string judgeStr, JoinEnum je = JoinEnum.LEFT )
        {
            string s = o1.GetType().FullName.Split('.')[1].Split('+')[0];
            this.f1 = s.Substring(0, s.Length - 3) + "∷" + o1.ToString();
            s = t.Name;
            this.f2 = s.Substring(0, s.Length - 3) + "∷" + "where";
            this.judgeStr = judgeStr;
            this.je = " " + je.ToString() + " JOIN ";
        }
    }

    public enum JoinEnum
    {
        LEFT,
        INNER,
        RIGHT,
    }
}
