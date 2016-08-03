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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tag.Vows.Interface;

/***
 * Thanks Newtonsoft  https://github.com/JamesNK/Newtonsoft.Json
 ***/

namespace Tag.Vows.Tool
{
    /// <summary>
    /// tools
    /// </summary>
    public class Tools : IComTools
    {
        /// <summary>
        /// 格式化时间（默认） 
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>经过格式化的时间</returns>
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
        /// <summary>
        /// 格式化时间（按指定格式）
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="format">格式</param>
        /// <returns>经过格式化的时间</returns>
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

        /// <summary>
        /// 格式化小数
        /// </summary>
        /// <param name="number">小数</param>
        /// <param name="format">格式</param>
        /// <returns>经过格式化的小数</returns>
        public string FloatFormat(object number, string format)
        {
            if (number == null)
            {
                return "";
            }
            string str = number.ToString();
            double d = 0;
            if (double.TryParse(str, out d))
            {
                return d.ToString(format);
            }
            return str;
        }
        /// <summary>
        /// 截取指定长度的字符串,'更多'用...
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">长度</param>
        /// <returns>若原符串串长度超过指定长度就从头截取该长度，否则原样返回</returns>
        public string SubString(object str, int length)
        {
            return SubString(str, length, "...");
        }
        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">长度</param>
        /// <param name="morestr">'更多'省略号</param>
        /// <returns>若原符串串长度超过指定长度就从头截取该长度，否则原样返回</returns>
        public string SubString(object str, int length, string morestr)
        {
            if (str == null)
            {
                return "";
            }
            if (length < 0)
            {
                return str.ToString();
            }
            string s = str.ToString().Trim();
            if (s.Length > length)
            {
                s = s.Substring(0, length) + morestr;
            }
            return s;
        }
        /// <summary>
        /// 将任意值转化为字符串
        /// </summary>
        /// <param name="obj">值</param>
        /// <returns>转化为字符串</returns>
        public string ValueOf(object obj)
        {
            if (obj == null)
            {
                return "null";
            }
            return obj.ToString();
        }

        /// <summary>
        /// 输出json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string JsonSerialize(object obj)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        /// <summary>
        /// 移除url中的 page = n 参数
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string RemovePageParams(string url)
        {
            string pageName = Regex.Match(url, "[^/]+$").Value;
            pageName = Regex.Replace(pageName, @"(?<=\?|&)page=[^&]*?(?=&|$)", string.Empty, RegexOptions.IgnoreCase);

            if (pageName.IndexOf("?") == -1)
            {
                pageName += "?";
            }
            else if (!pageName.EndsWith("?") && !pageName.EndsWith("&"))
            {
                pageName += "&";
            }
            pageName = Regex.Replace(pageName, @"&{2,}", "&", RegexOptions.IgnoreCase);
            return pageName;
        }

        /// <summary>
        /// 获取一级cookie
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value;
            }
            return "";
        }

        /// <summary>
        /// 获取二级cookie 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null
                && HttpContext.Current.Request.Cookies[strName] != null
                && HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                return HttpContext.Current.Request.Cookies[strName][key];
            }
            return "";
        }

        /// <summary>
        /// 将分割的字符串转换为数组
        /// </summary>
        /// <param name="arrStr">字符串</param>
        /// <param name="toType">数组数据类型</param>
        /// <returns>转换后的数组</returns>
        public List<string> StrToArray(string arrStr, string toType)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(arrStr) || string.IsNullOrEmpty(toType))
            {
                return list;
            }
            var arr = arrStr.Split(',');
            if (string.IsNullOrEmpty(toType) || toType.ToLower() == "string")
            {
                foreach (string s in arr)
                {
                    list.Add(s);
                }
                return list;
            }
            else if (toType == "Int16" || toType.ToLower() == "short"
                || toType == "Int32" || toType.ToLower() == "int"
                || toType == "Int64" || toType.ToLower() == "long")
            {
                Regex intTest = new Regex(@"^\-?\d+$");
                string num = "";
                foreach (string s in arr)
                {
                    num = s.Trim();
                    if (intTest.IsMatch(num))
                    {
                        list.Add(num);
                    }
                }
            }
            else if (toType.ToLower() == "double" || toType.ToLower() == "float"
                || toType.ToLower() == "decimal")
            {
                Regex floatTest = new Regex(@"^\-?\d+(\.\d+)?$");
                string num = "";
                foreach (string s in arr)
                {
                    num = s.Trim();
                    if (floatTest.IsMatch(num))
                    {
                        list.Add(num);
                    }
                }
            }
            else if (toType == "Boolean" || toType == "bool")
            {
                foreach (string s in arr)
                {
                    if (s.ToLower() == "true" || s == "1")
                    {
                        list.Add("true");
                    }
                    else
                    {
                        list.Add("false");
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 将分割的字符串转换为数组
        /// </summary>
        /// <param name="arrStr">字符串</param>
        /// <returns>转换后的数组</returns>
        public List<string> StrToArray(string arrStr)
        {
            return StrToArray(arrStr, "string");
        }
    }
}