using System;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CommonUtils
{
    public static class CommonUtil
    {
        public static string ToStr(this object v)
        {
            if (v is System.DBNull || v == null)
            {
                return string.Empty;
            }
            return Convert.ToString(v);
        }

        public static string ToStr(this object v, string reStr)
        {
            if (v is System.DBNull || v == null || v.Equals(""))
            {
                return reStr;
            }
            return Convert.ToString(v);
        }

        public static decimal ToDec(this object v)
        {
            if (v is System.DBNull || v == null)
            {
                return 0;
            }
            else if (Convert.ToString(v) == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(v);
            }
        }
        public static double ToDouble(this object v)
        {
            if (v is System.DBNull || v == null)
            {
                return 0;
            }
            else if (Convert.ToString(v) == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(v);
            }
        }

        public static int ToInt(this object v)
        {
            if (v is System.DBNull || v == null)
            {
                return 0;
            }
            else if (Convert.ToString(v) == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(v);
            }
        }

        public static long ToLong(this object v)
        {
            if (v is System.DBNull || v == null)
            {
                return 0;
            }
            else if (Convert.ToString(v) == "")
            {
                return 0;
            }
            else
            {
                long outPut = 0;
                bool result = long.TryParse(v.ToStr(), out outPut);
                return result ? outPut : long.MinValue; ;
            }
        }


        public static bool Bool(this object v)
        {
            string s = ToStr(v).ToLower();
            if (s == "true" || s == "1")
            {
                return true;
            }
            else if (s == "false" || s == "0")
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object v)
        {
            DateTime outDateTime;
            if (DateTime.TryParse(CommonUtil.ToStr(v), out outDateTime))
            {
                return outDateTime;
            }

            return DateTime.MinValue;
        }
        public static bool IsHaveData(DataSet ds)
        {
            if (ds == null)
            {
                return false;
            }
            else if (ds.Tables.Count <= 0)
            {
                return false;
            }
            else if (ds.Tables[0].Rows.Count <= 0)
            {
                return false;
            }

            return true;
        }

        public static bool IsHaveData(DataTable dt)
        {
            if (dt == null)
            {
                return false;
            }
            else if (dt.Rows.Count <= 0)
            {
                return false;
            }

            return true;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            if (value == null)
            {
                return true;
            }

            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 简化decimal变量，去除多余字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Simplify(this decimal value)
        {
            string result = value.ToStr();

            if (result.IndexOf('.') >= 0)
            {
                result = result.TrimEnd('.', '0');
            }

            return result;
        }

        /// <summary>
        /// 去除简化decimal变量多余的位数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string DecimalToString(this decimal d)
        {
            return d.ToString("#0.###############");
        }

        /// <summary>
        /// 正则匹配UTC日期   yyyy-MM-dd {-,/}
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDateByUTC(this string value)
        {
            return value.ToDateTime().ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 正则匹配GMT日期-测试阶段   Oct 12 2017 00:00:00
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime GetDateByGMT(this string value)
        {
            Regex reg = new Regex(@"[a-zA-Z]{3} \d{2} \d{4} \d{2}(\:)\d{2}(\:)\d{2}");
            Match m = reg.Match(value);

            if (m.Groups.Count > 0)
            {
                //切片
                string[] dateSplitString = m.Groups[0].ToStr().Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

                //6属性处理
                if (dateSplitString.Length == 6)
                {
                    int year = dateSplitString[2].ToInt();
                    int month;
                    int day = dateSplitString[1].ToInt();

                    int hour = dateSplitString[3].ToInt();
                    int minute = dateSplitString[4].ToInt();
                    int second = dateSplitString[5].ToInt();

                    //月份转换
                    switch (dateSplitString[0])
                    {
                        case "Jan":
                            month = 1;
                            break;
                        case "Feb":
                            month = 2;
                            break;
                        case "Mar":
                            month = 3;
                            break;
                        case "Apr":
                            month = 4;
                            break;
                        case "May":
                            month = 5;
                            break;
                        case "June":
                            month = 6;
                            break;
                        case "Jul":
                            month = 7;
                            break;
                        case "Aug":
                            month = 8;
                            break;
                        case "Sep":
                            month = 9;
                            break;
                        case "Oct":
                            month = 10;
                            break;
                        case "Nov":
                            month = 11;
                            break;
                        case "Dec":
                            month = 12;
                            break;
                        default:
                            throw new Exception(dateSplitString[0] + "月份非法！");
                    }

                    //处理字符串
                    DateTime date = new DateTime(year, month, day, hour, minute, second);

                    return date;
                }
            }

            throw new Exception("不是标准的格林时间");
        }

        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base64ToString(this string value)
        {
            byte[] bpath = Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(bpath);
        }

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringToBase64(this string value)
        {
            System.Text.Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(value);
            return Convert.ToBase64String(bytedata, 0, bytedata.Length);
        }

        /// <summary>
        /// 日期转字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 利用反射来判断对象是否包含某个属性
        /// </summary>
        /// <param name="instance">object</param>
        /// <param name="propertyName">需要判断的属性</param>
        /// <returns>是否包含</returns>
        public static bool ContainProperty(this object instance, string propertyName)
        {
            if (instance != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyInfo _findedPropertyInfo = instance.GetType().GetProperty(propertyName);
                return (_findedPropertyInfo != null);
            }
            return false;
        }
    }
}
