#region  The MIT License (MIT)
/*
The MIT License (MIT)

Copyright (c) 2015 ichynul

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tag.Vows.Interface;
/***
 * Thanks Newtonsoft  https://github.com/JamesNK/Newtonsoft.Json
 ***/

namespace Tag.Vows.Tool
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
                return "null";
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