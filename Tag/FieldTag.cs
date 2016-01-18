using System.Text.RegularExpressions;

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
                    this.Type = FieldType.request_value;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.SessionValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.session_value;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.CookieValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.cookie_value;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.ItemValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.item_value;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.ReadValue, RegexOptions.IgnoreCase))
                {
                    this.Type = FieldType.read_value;
                }
                else if (Regex.IsMatch(Obj, this.Path.tagregex.FormValue, RegexOptions.IgnoreCase))
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
            if (this.Type == FieldType.request_value)
            {
                return string.Format("<% = Request.QueryString[\"{0}\"] %>", mParams[1]);
            }
            else if (this.Type == FieldType.session_value)
            {
                return string.Format("<% = \"\" + Session[\"{0}\"] %>", mParams[1]);
            }
            else if (this.Type == FieldType.cookie_value)
            {
                return string.Format("<% = \"\" + Request.Cookies[\"{0}\"] %>", mParams[1]);
            }
            else if (this.Type == FieldType.item_value)
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
            else if (this.Type == FieldType.read_value)
            {
                if (!string.IsNullOrEmpty(this.Dataname))
                {
                    string name = mParams[0];
                    string itemField = TempleHelper.getTempleHelper(this.Path).getModFieldName(Dataname, mParams[1]);
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
                return string.Format("<% ={0} %>", this.Obj);
            }
        }

        public override string ToTagString()
        {
            return "【全局名称" + this.GetTagName() + ",标签类型：filed，全名：" + this.Obj + "】<br />";
        }

        public void SetDataName(string DataName, FieldType type)
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