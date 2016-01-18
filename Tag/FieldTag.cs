﻿using System.Text.RegularExpressions;

namespace Tag.Vows
{
    class FieldTag : BaseTag, IFieldDataAble
    {
        private string Obj;
        private string Dataname;
        private string[] mParams;
        public FieldType Type { get; private set; }
        public FieldTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {
        }

        protected override void Discover()
        {
            this.Obj = this.Path.tagregex.getFiledObj(this.Text);
            if (this.Obj.IndexOf(".") != -1)
            {
                mParams = this.Obj.Split('.');
                if (Regex.IsMatch(Obj, this.Path.tagregex.RequestValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.requestValue;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.SessionValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.sessionValue;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.CookieValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.cookieValue;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.ItemValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.itemValue;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.ReadValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.readValue;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.FormValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.formValue;
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
            if (this.Type == FieldType.requestValue)
            {
                return string.Format("<% = Request.QueryString[\"{0}\"] %>", mParams[1]);
            }
            else if (this.Type == FieldType.sessionValue)
            {
                return string.Format("<% = \"\" + Session[\"{0}\"] %>", mParams[1]);
            }
            else if (this.Type == FieldType.cookieValue)
            {
                return string.Format("<% = \"\" + Request.Cookies[\"{0}\"] %>", mParams[1]);
            }
            else if (this.Type == FieldType.itemValue)
            {
                if (!string.IsNullOrEmpty(this.Dataname))
                {
                    string name = mParams[0];
                    string itemField = TempleHelper.getTempleHelper(this.Path).getModFieldName(Dataname, mParams[1]);
                    return string.Format("<% ={0}.{1} %>", name, itemField);
                }
                else
                {
                    return string.Format("<%# Eval(\"{0}\") %>",
                    Regex.Replace(Obj, @"(item|\.|\{|\})", string.Empty, RegexOptions.IgnoreCase)
                    );
                }
            }
            else if (this.Type == FieldType.readValue)
            {
                if (!string.IsNullOrEmpty(this.Dataname))
                {
                    string name = mParams[0];
                    string itemField = TempleHelper.getTempleHelper(this.Path).getModFieldName(Dataname, mParams[1]);
                    return string.Format("<% ={0}.{1} %>", name, itemField);
                }
                return this.Text;
            }
            else if (this.Type == FieldType.formValue)
            {
                return mParams[1];
            }
            else
            {
                return string.Format("<% ={0} %>", this.Obj);
            }
        }

        public override string ToTagString()
        {
            return "【全局名称" + this.GetTagName() + ",标签类型：filed，全名：" + this.Obj + "】<br />";
        }

        public void setDataName(string DataName, FieldType type)
        {
            if (this.Type == type)
            {
                this.Dataname = DataName;
            }
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

        public string getFieldName()
        {
            if (this.Type == FieldType.itemValue)
            {
                return Regex.Replace(Obj, @"(item|\.|\{|\})", string.Empty, RegexOptions.IgnoreCase);
            }
            return null;
        }
    }
}