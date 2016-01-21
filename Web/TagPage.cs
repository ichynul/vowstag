/*
* The MIT License (MIT)

*Copyright (c) 2015 ichynul

*Permission is hereby granted, free of charge, to any person obtaining a copy
*of this software and associated documentation files (the "Software"), to deal
*in the Software without restriction, including without limitation the rights
*to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*copies of the Software, and to permit persons to whom the Software is
*furnished to do so, subject to the following conditions:

*The above copyright notice and this permission notice shall be included in all
*copies or substantial portions of the Software.

*THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
*LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
*OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
*SOFTWARE.
*/

using System;
using System.Collections.Specialized;
using Tag.Vows.Web;
using Tag.Vows.Interface;
using Tag.Vows.Tool;
using System.Web.UI;

namespace Tag.Vows.Web
{
    public class TagPage : System.Web.UI.Page, IComTools, ICallBackControl, ICallbackEventHandler
    {
        /// <summary>
        /// 用于保存站点及页面的通用信息
        /// </summary>
        protected dynamic config;
        /// <summary>
        /// callBack 时请求参数
        /// </summary>
        protected string _callBackstr;
        private int _page;
        private Tools _tools;
        private NameValueCollection _CallString;

        public string TimeFormat(object time)
        {
            return tools.TimeFormat(time);
        }

        public string TimeFormat(object time, string format)
        {
            return tools.TimeFormat(time, format);
        }

        public string FloatFormat(object number, string format)
        {
            return tools.FloatFormat(number, format);
        }

        public string SubString(object str, int length)
        {
            return tools.SubString(str, length);
        }

        public string ValueOf(object obj)
        {
            return tools.ValueOf(obj);
        }

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

        protected NameValueCollection CallString
        {
            get
            {
                if (this._CallString == null)
                {
                    _CallString = new NameValueCollection();
                    if (!string.IsNullOrEmpty(this._callBackstr))
                    {
                        string[] arr = this._callBackstr.Split('&');
                        foreach (var kv in arr)
                        {
                            string[] v = kv.Split('=');
                            if (v.Length == 2)
                            {
                                _CallString.Add(v[0], v[1]);
                            }
                        }
                    }
                }
                return this._CallString;
            }
        }

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
                    call = new CallBackResult(new { code = "1", msg = "出错了！\n" + ex.ToString() });
                    call.type = "error";
                }
            }
            return tools.JsonSerialize(call);
        }

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

        protected virtual object GetDbObject()
        {
            return null;
        }
    }
}

