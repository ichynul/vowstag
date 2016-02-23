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
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web.UI;
using Tag.Vows.Interface;
using Tag.Vows.Tool;
using System.Dynamic;

namespace Tag.Vows.Web
{
    /// <summary>
    /// CallbackResult异常处理
    /// </summary>
    /// <param name="ex">异常</param>
    public delegate void CatchException(Exception ex);
    /// <summary>
    /// tag页面继承此类
    /// </summary>
    public class TagPage : System.Web.UI.Page, IComTools, ICallBackControl, ICallbackEventHandler
    {
        /// <summary>
        /// 当CallbackResultException时处理异常
        /// </summary>
        protected CatchException OnCallbackException = ex => { };
        /// <summary>
        /// 用于保存站点及页面的通用信息
        /// </summary>
        protected dynamic config = new ExpandoObject();
        /// <summary>
        /// callBack 时请求参数
        /// </summary>
        protected string _callBackstr;
        private int _page;
        private Tools _tools;
        /// <summary>
        /// 在Page_Load里加载各标签前判断，若返回false则标签都不会加载
        /// </summary>
        protected Func<bool> Befor_Load_Tags = () => true;

        private NameValueCollection _CallString;

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
        /// 将任意值转化为字符串
        /// </summary>
        /// <param name="obj">值</param>
        /// <returns>转化为字符串</returns>
        public string ValueOf(object obj)
        {
            return tools.ValueOf(obj);
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

        internal Tools tools
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
        /// callBack请求的键值对
        /// </summary>
        protected NameValueCollection CallString
        {
            get
            {
                if (this._CallString == null)
                {
                    _CallString = new NameValueCollection();
                    if (!string.IsNullOrEmpty(this._callBackstr))
                    {
                        var matches = Regex.Matches(_callBackstr, @"(?<=^|&)\s*(?<key>\w+)\s*=(?<value>.*?)(?=&|$)", RegexOptions.Singleline);
                        foreach (Match kv in matches)
                        {
                            _CallString.Add(kv.Groups["key"].Value, kv.Groups["value"].Value);
                        }
                    }
                }
                return this._CallString;
            }
        }

        /// <summary>
        /// 用于获取一个tag或用法发起的callack请求结果
        /// </summary>
        /// <returns>CallBackResult</returns>
        public string GetCallbackResult()
        {
            CallBackResult call = TagCallBack();
            if (call == null)
            {
                try
                {
                    call = DoCallBack();
                    if (call != null)
                    {
                        call.type = "mycall";
                    }
                    else
                    {
                        call = new CallBackResult(new
                        {
                            code = "1",
                            msg = string.Concat("服务端已收到：", this._callBackstr,
                            "。若要处理本请求，服务端处理类需覆盖(override)方法 public CallBackResult DoCallBack")
                        });
                    }
                }
                catch (Exception ex)
                {
                    call = new CallBackResult(new { code = "1", msg = "出错了！-" + ex.Message });
                    call.type = "error";
                    OnCallbackException(ex);
                }
            }
            return tools.JsonSerialize(call);
        }

        /// <summary>
        /// 处理 CallBack
        /// </summary>
        /// <param name="eventArgument">请求参数</param>
        public void RaiseCallbackEvent(string eventArgument)
        {
            this._callBackstr = eventArgument;
        }

        /// <summary>
        /// 用于从查询 callBack时 arg 参数值
        /// </summary>
        /// <param name="key">key值</param>
        /// <returns>value值</returns>
        public string CallValue(string key)
        {
            return CallString[key];
        }

        /// <summary>
        /// 重写本方法以处理自定义的callback
        /// </summary>
        /// <returns>处理结果</returns>
        public virtual CallBackResult DoCallBack()
        {
            return new CallBackResult(new { code = "1", msg = "什么也不做！", });
        }

        /// <summary>
        /// 供 JsonTag和FormTag 重写，请勿手动重写本方法
        /// </summary>
        /// <returns>处理结果</returns>
        public virtual CallBackResult TagCallBack()
        {
            return null;
        }

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

