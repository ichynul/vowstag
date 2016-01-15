using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
/***
 * Thanks Newtonsoft  https://github.com/JamesNK/Newtonsoft.Json
 ***/

namespace Tag.Vows
{
    class Tools : IComTools
    {
        public Tools() { }

        public string TimeFormat(object time)
        {
            if (time == null)
            {
                return "";
            }
            string str = time.ToString();
            DateTime dt = DateTime.Now;
            if (DateTime.TryParse(str, out dt))
            {
                return dt.ToString("yyyy年MM月dd日");
            }
            return str;
        }

        public string TimeFormat(object time, string format)
        {
            if (time == null)
            {
                return "";
            }
            if (string.IsNullOrEmpty(format))
            {
                return TimeFormat(time);
            }
            string str = time.ToString();
            DateTime dt = DateTime.Now;
            if (DateTime.TryParse(str, out dt))
            {
                return dt.ToString(format);
            }
            return str;
        }

        public string FloatFormat(object number, string format)
        {
            if (number == null)
            {
                return "";
            }
            string str = number.ToString();
            DateTime dt = DateTime.Now;
            if (DateTime.TryParse(str, out dt))
            {
                return dt.ToString(format);
            }
            return str;
        }

        public string SubString(object str, int length)
        {
            if (str == null)
            {
                return "";
            }
            string s = str.ToString().Trim();
            if (s.Length > length)
            {
                s = s.Substring(0, length) + "...";
            }
            return s;
        }

        public string ValueOf(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString();
        }

        public string JsonSerialize(object obj)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public string RemovePageParams(string url)
        {
            string pageName = Regex.Match(url, "[^/]+$").Value;
            return Regex.Replace(pageName, @"&?page=[^&]*(?=&|$)", string.Empty, RegexOptions.IgnoreCase);
        }
    }
}