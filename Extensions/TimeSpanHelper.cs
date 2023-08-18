using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiApp.Extensions
{
    public static class TimeSpanHelper
    {
        /// <summary>
        /// 日期 轉 時間戳記
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTime(DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        /// <summary>
        /// 時間戳記 轉 日期
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(long unixTime)
        {
            return new DateTime(1970, 1, 1).ToLocalTime().AddSeconds(unixTime);
        }

        /// <summary>
        /// 取得兩個時間的差距秒數
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static double GetTimeSpanDuration(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime).TotalSeconds;
        }

        /// <summary>
        /// 是否是時間戳記
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTimeSpan(string value)
        {
            return TimeSpan.TryParse(value, out _);
        }

    }
}