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
    ///�� label �� sublist ʹ�õ�ҳ��Ƭ��
    /// </summary>
    public abstract class SubControl : UserControl, IComTools
    {
        /// <summary>
        /// ���ڱ���վ�㼰ҳ���ͨ����Ϣ
        /// </summary>
        protected dynamic config;
        private int _page;
        private Tools _tools;
        /// <summary>
        /// ��Page_Load����ظ���ǩǰ�жϣ�������false���ǩ���������
        /// </summary>
        protected Func<bool> Befor_Load_Tags = () => { return true; };
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
        /// ��ȡָ�����ȵ��ַ���
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
        /// <param name="morestr">����..ʡ�Ժ�</param>
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
        /// SetConfig
        /// </summary>
        /// <param name="_config">_config</param>
        public void SetConfig(dynamic _config)
        {
            this.config = _config;
        }

        /// <summary>
        /// Ϊsublistָ����һ����ʵ��
        /// </summary>
        /// <param name="mItem">mItem</param>
        public virtual void SetItem(object mItem) { }

        /// <summary>
        /// ָ��Eitities ʵ��
        /// </summary>
        /// <param name="db">Eitities db</param>
        public virtual void SetDb(object db) { }

        /// <summary>
        /// ���ص�ǰ�� Eitities db ʵ��
        /// </summary>
        /// <returns></returns>
        protected virtual object GetDbObject()
        {
            return null;
        }
    }
}