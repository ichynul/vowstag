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
using System.Web.UI;
using Tag.Vows.Interface;
using Tag.Vows.Tool;

namespace Tag.Vows.Web
{
    /// <summary>
    ///供 label 或 sublist 使用的页面片段
    /// </summary>
    public abstract class SubControl : UserControl, IComTools
    {
        /// <summary>
        /// 用于保存站点及页面的通用信息
        /// </summary>
        protected dynamic config;
        private int _page;
        private Tools _tools;
        /// <summary>
        /// 在Page_Load里加载各标签前判断，若返回false则标签都不会加载
        /// </summary>
        protected Func<bool> Befor_Load_Tags = () => { return true; };
        /// <summary>
        /// 格式化时间（默认） 
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>经过格式化的时间</returns>
        public string TimeFormat(object time)
        {
            return tools.TimeFormat(time);
        }
        /// <summary>
        /// 格式化时间（按指定格式）
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="format">格式</param>
        /// <returns>经过格式化的时间</returns>
        public string TimeFormat(object time, string format)
        {
            return tools.TimeFormat(time, format);
        }

        /// <summary>
        /// 格式化小数
        /// </summary>
        /// <param name="number">小数</param>
        /// <param name="format">格式</param>
        /// <returns>经过格式化的小数</returns>
        public string FloatFormat(object number, string format)
        {
            return tools.FloatFormat(number, format);
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">长度</param>
        /// <returns>若原符串串长度超过指定长度就从头截取该长度，否则原样返回</returns>
        public string SubString(object str, int length)
        {
            return tools.SubString(str, length);
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">长度</param>
        /// <param name="morestr">更多..省略号</param>
        /// <returns>若原符串串长度超过指定长度就从头截取该长度，否则原样返回</returns>
        public string SubString(object str, int length, string morestr)
        {
            return tools.SubString(str, length, morestr);
        }

        /// <summary>
        /// 将任意值转化为字符串
        /// </summary>
        /// <param name="obj">值</param>
        /// <returns>转化为字符串</returns>
        public string ValueOf(object obj)
        {
            return tools.ValueOf(obj);
        }

        /// <summary>
        /// 获取一级cookie
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string GetCookie(string strName)
        {
            return tools.GetCookie(strName);
        }

        /// <summary>
        /// 获取二级cookie 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetCookie(string strName, string key)
        {
            return tools.GetCookie(strName, key);
        }

        /// <summary>
        /// 输出json
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>jsonstr</returns>
        public string JsonSerialize(object obj)
        {
            return this.tools.JsonSerialize(obj);
        }

        /// <summary>
        /// 移除url中的 page = n 参数
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string RemovePageParams(string url)
        {
            return tools.RemovePageParams(url);
        }

        /// <summary>
        /// 将分割的字符串转换为数组
        /// </summary>
        /// <param name="arrStr">字符串</param>
        /// <param name="toType">数组数据类型</param>
        /// <returns>转换后的数组</returns>
        public List<string> StrToArray(string arrStr, string toType)
        {
            return tools.StrToArray(arrStr, toType);
        }

        /// <summary>
        /// 将分割的字符串转换为数组
        /// </summary>
        /// <param name="arrStr">字符串</param>
        /// <returns>转换后的数组</returns>
        public List<string> StrToArray(string arrStr)
        {
            return tools.StrToArray(arrStr, "string");
        }

        /// <summary>
        /// 页码
        /// </summary>
        protected int page
        {
            get
            {
                if (_page < 1)
                {
                    int.TryParse(Request.QueryString["page"], out _page);
                    _page = _page < 1 ? 1 : _page;
                }
                return _page;
            }
        }

        /// <summary>
        /// tools
        /// </summary>
        protected Tools tools
        {
            get
            {
                if (_tools == null)
                {
                    _tools = new Tools();
                }
                return _tools;
            }
        }

        /// <summary>
        /// SetConfig
        /// </summary>
        /// <param name="_config">_config</param>
        public void SetConfig(dynamic _config)
        {
            this.config = _config;
        }

        /// <summary>
        /// 为sublist指定上一级的实例
        /// </summary>
        /// <param name="mItem">mItem</param>
        public virtual void SetItem(object mItem) { }

        /// <summary>
        /// 指定Eitities 实例
        /// </summary>
        /// <param name="db">Eitities db</param>
        public virtual void SetDb(object db) { }

        /// <summary>
        /// 返回当前的 Eitities db 实例
        /// </summary>
        /// <returns></returns>
        protected virtual object GetDbObject()
        {
            return null;
        }
    }
}