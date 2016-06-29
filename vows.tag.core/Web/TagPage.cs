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
using System.Collections.Specialized;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Web.UI;
using Tag.Vows.Interface;
using Tag.Vows.Tool;

namespace Tag.Vows.Web
{

    /// <summary>
    /// tagҳ��̳д���
    /// </summary>
    public class TagPage : System.Web.UI.Page, IComTools, ICallBackControl, ICallbackEventHandler
    {
        /// <summary>
        /// ���ڱ���վ�㼰ҳ���ͨ����Ϣ
        /// </summary>
        protected dynamic config = new ExpandoObject();
        /// <summary>
        /// callBack ʱ�������
        /// </summary>
        protected string _callBackstr;
        private int _page;
        private Tools _tools;
        /// <summary>
        /// ��Page_Load����ظ���ǩǰ�жϣ�������false���ǩ���������
        /// </summary>
        protected Func<bool> Befor_Load_Tags = () => true;

        private NameValueCollection _CallString;

        /// <summary>
        /// ��ʽ��ʱ�䣨Ĭ�ϣ� 
        /// </summary>
        /// <param name="time">ʱ��</param>
        /// <returns>������ʽ����ʱ��</returns>
        public string TimeFormat(object time)
        {
            return tools.TimeFormat(time);
        }
        /// <summary>
        /// ��ʽ��ʱ�䣨��ָ����ʽ��
        /// </summary>
        /// <param name="time">ʱ��</param>
        /// <param name="format">��ʽ</param>
        /// <returns>������ʽ����ʱ��</returns>
        public string TimeFormat(object time, string format)
        {
            return tools.TimeFormat(time, format);
        }

        /// <summary>
        /// ��ʽ��С��
        /// </summary>
        /// <param name="number">С��</param>
        /// <param name="format">��ʽ</param>
        /// <returns>������ʽ����С��</returns>
        public string FloatFormat(object number, string format)
        {
            return tools.FloatFormat(number, format);
        }

        /// <summary>
        /// ��ȡָ�����ȵ��ַ���,'����'��...
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="length">����</param>
        /// <returns>��ԭ���������ȳ���ָ�����Ⱦʹ�ͷ��ȡ�ó��ȣ�����ԭ������</returns>
        public string SubString(object str, int length)
        {
            return tools.SubString(str, length);
        }

        /// <summary>
        /// ��ȡָ�����ȵ��ַ���
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="length">����</param>
        /// <param name="morestr">'����'ʡ�Ժ�</param>
        /// <returns>��ԭ���������ȳ���ָ�����Ⱦʹ�ͷ��ȡ�ó��ȣ�����ԭ������</returns>
        public string SubString(object str, int length, string morestr)
        {
            return tools.SubString(str, length, morestr);
        }

        /// <summary>
        /// ������ֵת��Ϊ�ַ���
        /// </summary>
        /// <param name="obj">ֵ</param>
        /// <returns>ת��Ϊ�ַ���</returns>
        public string ValueOf(object obj)
        {
            return tools.ValueOf(obj);
        }
        /// <summary>
        /// ���json
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>jsonstr</returns>
        public string JsonSerialize(object obj)
        {
            return this.tools.JsonSerialize(obj);
        }

        /// <summary>
        /// �Ƴ�url�е� page = n ����
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string RemovePageParams(string url)
        {
            return tools.RemovePageParams(url);
        }

        /// <summary>
        /// ��ȡһ��cookie
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string GetCookie(string strName)
        {
            return tools.GetCookie(strName);
        }

        /// <summary>
        /// ��ȡ����cookie 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetCookie(string strName, string key)
        {
            return tools.GetCookie(strName, key);
        }

        /// <summary>
        /// ���ָ���ַ���ת��Ϊ����
        /// </summary>
        /// <param name="arrStr">�ַ���</param>
        /// <param name="toType">������������</param>
        /// <returns>ת���������</returns>
        public List<string> StrToArray(string arrStr, string toType)
        {
            return tools.StrToArray(arrStr, toType);
        }

        /// <summary>
        /// ���ָ���ַ���ת��Ϊ����
        /// </summary>
        /// <param name="arrStr">�ַ���</param>
        /// <returns>ת���������</returns>
        public List<string> StrToArray(string arrStr)
        {
            return tools.StrToArray(arrStr, "string");
        }

        /// <summary>
        /// ҳ��
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
        /// ���ڻ�ȡһ��tag���÷������callack������
        /// </summary>
        /// <returns>CallbackResult</returns>
        public string GetCallbackResult()
        {
            CallbackResult call = TagCallback();
            if (call == null)
            {
                call = BeforDoCallback();
                if (call != null)
                {
                    call.type = "mycall";
                }
            }
            if (call == null)
            {
                try
                {
                    call = DoCallback();
                    if (call != null)
                    {
                        call.type = "mycall";
                    }
                    else
                    {
                        call = new CallbackResult(new
                        {
                            code = "1",
                            msg = string.Concat("��������յ���", this._callBackstr,
                            "����Ҫ�������󣬷���˴������踲��(override)���� public CallbackResult DoCallback"),
                            type = "none"
                        });
                    }
                }
                catch (Exception ex)
                {
                    call = new CallbackResult(new { code = "1", msg = "�����ˣ�-" + ex.Message });
                    call.type = "error";
                    CallbackException(ex);
                }
            }
            if (call != null)
            {
                if (call.type != "error")
                {
                    call.callstr = this._callBackstr;
                }
                call.pageName = this.GetType().BaseType.BaseType.Name;
            }
            return tools.JsonSerialize(call);
        }

        /// <summary>
        /// ���� CallBack
        /// </summary>
        /// <param name="eventArgument">�������</param>
        public void RaiseCallbackEvent(string eventArgument)
        {
            this._callBackstr = eventArgument;
        }

        /// <summary>
        /// ���ڴӲ�ѯ callBackʱ arg ����ֵ
        /// </summary>
        /// <param name="key">keyֵ</param>
        /// <returns>valueֵ</returns>
        public string CallValue(string key)
        {
            return CallString[key];
        }

        /// <summary>
        /// callBack����ļ�ֵ��
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
        /// ��д�������Դ����Զ����callback
        /// </summary>
        /// <returns>������</returns>
        public virtual CallbackResult DoCallback()
        {
            return new CallbackResult(new { code = "1", msg = "ʲôҲ������", });
        }

        /// <summary>
        /// ��д��������ʵ��DoCallbackǰ�Ĳ����������������ط�nullֵ����DoCallback������ִ��
        /// </summary>
        /// <returns>������</returns>
        public virtual CallbackResult BeforDoCallback()
        {
            return null;
        }

        /// <summary>
        /// �� JsonTag��FormTag ��д�������ֶ���д������
        /// </summary>
        /// <returns>������</returns>
        public virtual CallbackResult TagCallback()
        {
            return null;
        }

        /// <summary>
        /// ���ص�ǰ�� Eitities db ʵ��
        /// </summary>
        /// <returns></returns>
        protected virtual object GetDbObject()
        {
            return null;
        }

        /// <summary>
        /// ��CallbackResult �����쳣 Exception
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void CallbackException(Exception ex)
        {
            return;
        }
    }
}

