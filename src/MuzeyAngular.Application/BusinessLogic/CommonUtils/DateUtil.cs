using System;
using System.Collections.Generic;
using System.Globalization;

namespace CommonUtils
{
    public static class DateUtil
    {
        public const string DateFormatYYYYMMDD = "yyyyMMdd";

        public const string DateFormatDisplayYYYYMMDD = "yyyy-MM-dd";

        public const string DateFormatYYYYMM = "yyyyMM";

        public const string DateFormatDisplayYYYYMM = "yyyy-MM-dd";

        public const string DateFormatFull = "yyyy-MM-dd HH:mm:ss";

        public const string DateFormatDateTime = "yyyy-MM-dd HH:mm";

        public static string Format(DateTime dt, string format)
        {
            return dt.ToString(format);
        }

        public static string FormatToDate(string date)
        {
            string year = date.Substring(0, 4);
            string month = date.Substring(4, 2);
            string day = date.Substring(6, 2);
            string hour = date.Substring(8, 2);
            string min = date.Substring(10, 2);

            return string.Format("{0}-{1}-{2} {3}:{4}", year, month, day, hour, min);
        }

        /// <summary>
        /// 获取某年所有星期的周一和周日的日期List下标+1为第N周
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static List<string[]> GetYearWeekSE(int year)
        {
            List<string[]> resList = new List<string[]>();
            int weekDay;
            string[] dateSE = new string[2];

            DateTime time = new DateTime(year, 1, 1);
            DateTime timeEnd = new DateTime(year + 1, 1, 1);

            dateSE[0] = String.Format("{0:yyyyMMdd}", time);

            while (DateTime.Compare(time, timeEnd) != 0)
            {
                weekDay = Convert.ToInt32(time.DayOfWeek.ToString("d"));
                if (weekDay == 1)
                {
                    dateSE = new string[2];
                    dateSE[0] = String.Format("{0:yyyyMMdd}", time);
                }
                else if (weekDay == 0)
                {
                    dateSE[1] = String.Format("{0:yyyyMMdd}", time);
                    resList.Add(dateSE);
                }

                time = time.AddDays(1);
            }

            time = time.AddDays(-1);
            weekDay = Convert.ToInt32(time.DayOfWeek.ToString("d"));
            if (weekDay != 0)
            {
                dateSE[1] = String.Format("{0:yyyyMMdd}", time);
                resList.Add(dateSE);
            }

            return resList;
        }

        /// <summary>
        /// 获取该日期所在周的周一到周日的日期列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDaysOfThisWeek(DateTime someDate)
        {
            if (someDate == null || someDate.Equals(DateTime.MinValue))
            {
                someDate = DateTime.Now;
            }

            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1)
            {
                i = 6;
            }
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);

            var list = new List<string>();
            DateTime monday = someDate.Subtract(ts);

            list.Add(monday.ToString("yyyy-MM-dd"));
            for (int j = 1; j <= 6; j++)
            {
                DateTime date = monday.AddDays(j);
                list.Add(date.ToString("yyyy-MM-dd"));
            }

            return list;
        }

        /// <summary>
        /// 获取指定日期，在为一年中为第几周
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <reutrn>返回第几周</reutrn>
        public static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        /// <summary>
        /// 获取该月份的最后一天
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime GetLastDateForMonth(int year, int month)
        {
            var date = new DateTime(year, month, 1);
            date = date.AddMonths(1);
            date = date.AddDays(-1);
            return date;
        }

        /// <summary>
        /// 已重载.计算两个日期的时间间隔,返回的是时间间隔的日期差的绝对值.
        /// </summary>
        /// <param name="DateTime1">第一个日期和时间</param>
        /// <param name="DateTime2">第二个日期和时间</param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString() + "天"
                        + ts.Hours.ToString() + "小时"
                        + ts.Minutes.ToString() + "分钟"
                        + ts.Seconds.ToString() + "秒";
            }
            catch
            {
                throw (new Exception("日期间隔计算失败"));
            }
            return dateDiff;
        }

        /// <summary>
        /// 已重载.计算两个日期的时间间隔,返回的是时间间隔的日期差的绝对值.
        /// </summary>
        /// <param name="DateTime1">第一个日期和时间</param>
        /// <param name="DateTime2">第二个日期和时间</param>
        /// <param name="type">Days,Hours,Minutes,Seconds</param>
        /// <returns></returns>
        public static int DateDiff(DateTime DateTime1, DateTime DateTime2, string type)
        {
            int dateDiff;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                var tp = typeof(TimeSpan).GetProperty(type);
                dateDiff = tp.GetValue(ts).ToInt();
            }
            catch
            {
                throw (new Exception("日期间隔计算失败"));
            }
            return dateDiff;
        }
    }
}
