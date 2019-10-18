using DotnetSpider.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderSample.Common
{
    class BaseFunction
    {

        /// <summary> 
        /// 获取当前时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        public static decimal TryParseDecimal(string value)
        {
            value = value.Trim();
            decimal de = 0;
            decimal.TryParse(value,out de);
            return de;
        }

        public static int TryParseInt(string value)
        {
            value = value.Trim();
            int de = 0;
            int.TryParse(value, out de);
            return de;
        }

        public static DateTime TryParseDateTime(string value)
        {
            value = value.Trim();
            DateTime de = DateTime.Now;
            DateTime.TryParse(value, out de);
            return de;
        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTimeFromTimeStamp(long timeStamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            return startTime.AddMilliseconds(timeStamp);
        }

        public static Request CreateRequest(string url)
        {
            Request request = new Request(url);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36";
            return request;
        }
    }
}
