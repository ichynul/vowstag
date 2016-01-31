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
using System.Text.RegularExpressions;
using Tag.Vows.Interface;
using Tag.Vows.Enum;
using Tag.Vows.Data;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class FieldTag : BaseTag, IFieldDataAble
    {
        private string Obj;
        private string Dataname;
        private string[] mParams;
        public FieldType Type { get; private set; }
        public FieldTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {
        }

        protected override void Discover()
        {
            this.Obj = this.Config.tagregex.getFiledObj(this.Text);
            if (this.Obj.IndexOf(".") != -1)
            {
                mParams = this.Obj.Split('.');
                if (this.Config.tagregex.RequestValue.IsMatch(Obj))
                {
                    this.Type = FieldType.request_value;
                }
                else if (this.Config.tagregex.SessionValue.IsMatch(Obj))
                {
                    this.Type = FieldType.session_value;
                }
                else if (this.Config.tagregex.CookieValue.IsMatch(Obj))
                {
                    this.Type = FieldType.cookie_value;
                }
                else if (this.Config.tagregex.ItemValue.IsMatch(Obj))
                {
                    this.Type = FieldType.item_value;
                }
                else if (this.Config.tagregex.ReadValue.IsMatch(Obj))
                {
                    this.Type = FieldType.read_value;
                }
                else if (this.Config.tagregex.FormValue.IsMatch(Obj))
                {
                    this.Type = FieldType.form_value;
                }
                else
                {
                    this.Type = FieldType.normal;
                }
            }
            else
            {
                if (Obj.ToLower() == "page")
                {
                    this.Type = FieldType.page;
                }
                else
                {
                    this.Type = FieldType.normal;
                }
            }
        }

        protected override string GetCodeForAspx()
        {

            if (this.Type == FieldType.page)
            {
                return "<% = page %>";
            }
            else if (this.Type == FieldType.request_value)
            {
                return string.Format("<% = Request.QueryString[\"{0}\"] %>", mParams[1]);
            }
            else if (this.Type == FieldType.session_value)
            {
                return string.Format("<% =  Session[\"{0}\"] %>", mParams[1]);
            }
            else if (this.Type == FieldType.cookie_value)
            {
                return string.Format("<% = Request.Cookies[\"{0}\"] %>", mParams[1]);
            }
            else if (this.Type == FieldType.item_value)
            {
                if (!string.IsNullOrEmpty(this.Dataname))
                {
                    string name = mParams[0];
                    string itemField = TempleHelper.getTempleHelper(this.Config).GetModFieldName(Dataname, mParams[1]);
                    return string.Format("<% = {0}.{1} %>", name, itemField);
                }
                else
                {
                    return string.Format("<%# Eval(\"{0}\") %>",
                    Regex.Replace(Obj, @"(item|\.|\{|\})", string.Empty, RegexOptions.IgnoreCase)
                    );
                }
            }
            else if (this.Type == FieldType.read_value)
            {
                if (!string.IsNullOrEmpty(this.Dataname))
                {
                    string name = mParams[0];
                    string itemField = TempleHelper.getTempleHelper(this.Config).GetModFieldName(Dataname, mParams[1]);
                    return string.Format("<% ={0}.{1} %>", name, itemField);
                }
                return this.Text;
            }
            else if (this.Type == FieldType.form_value)
            {
                return mParams[1];
            }
            else
            {
                return string.Format("<% = {0} %>", this.Obj);
            }
        }

        public override string ToTagString()
        {
            return "【全局名称" + this.GetTagName() + ",标签类型：filed，全名：" + this.Obj + "】<br />";
        }



        internal string getParamAt(int index)
        {
            if (mParams.Length > index)
            {
                return mParams[index];
            }
            return "";
        }

        internal void setParamAt(int index, string newValue)
        {
            if (mParams.Length > index)
            {
                mParams[index] = newValue;
            }
        }

        public void SetDataName(string DataName, FieldType type)
        {
            if (this.Type == type)
            {
                this.Dataname = DataName;
            }
        }

        public string GetFieldName()
        {
            if (this.Type == FieldType.item_value)
            {
                return Regex.Replace(Obj, @"(item|\.|\{|\})", string.Empty, RegexOptions.IgnoreCase);
            }
            return null;
        }
    }
}