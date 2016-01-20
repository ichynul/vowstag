using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Tag.Vows.Interface;
using Tag.Vows.Enum;
using Tag.Vows.Data;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class MethodTag : BaseTag, IMethodDataAble
    {
        private string Obj;
        private string Params;
        private string Method;
        private string ReadDataname = "";
        public MethodType Type { get; private set; }
        public string forname { get; set; }
        public MethodTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {
        }

        protected override void Discover()
        {
            this.Obj = this.Path.tagregex.getMethodObj(this.Text);
            string[] arr = Obj.Split('(');
            this.Method = arr[0];
            this.Params = arr[1].Replace(")", string.Empty);
            if (Regex.IsMatch(Obj, this.Path.tagregex.ReadValue, RegexOptions.IgnoreCase))
            {
                this.Type = MethodType.read_value_method;
            }
            else if (Regex.IsMatch(Obj, this.Path.tagregex.FormValue, RegexOptions.IgnoreCase))
            {
                this.Type = MethodType.form_method;
            }
            else
            {
                this.Type = MethodType.normal;
            }
        }

        protected override string GetCodeForAspx()
        {
            if (this.Type == MethodType.form_method)
            {
                return string.Format("_tagcall.form('{0}'{1}); return false;", this.forname, this.Params.ToLower() == "false" ? " ,false" : " ,true");
            }
            if (!string.IsNullOrEmpty(this.ReadDataname) && this.Type == MethodType.read_value_method)
            {
                var resdMatches = Regex.Matches(Obj, this.Path.tagregex.ReadValue, RegexOptions.IgnoreCase);
                if (resdMatches.Count > 0)
                {
                    ReadDataname = TempleHelper.getTempleHelper(this.Path).GetTableName(ReadDataname);
                    string itemField = "";
                    foreach (Match m in resdMatches)
                    {
                        itemField = TempleHelper.getTempleHelper(this.Path).GetModFieldName(ReadDataname, m.Value.Split('.')[1]);
                        if (!string.IsNullOrEmpty(itemField))
                        {
                            this.Obj = this.Obj.Replace(m.Value, string.Concat("read.", itemField ));
                        }
                    }
                }
            }
            var matches = Regex.Matches(Obj, this.Path.tagregex.ItemValue, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Obj = this.Obj.Replace(m.Value, string.Concat("Eval(\"" , m.Value.Split('.')[1] , "\")"));
                }
            }
            matches = Regex.Matches(Obj, this.Path.tagregex.SessionValue, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Obj = this.Obj.Replace(m.Value, string.Concat("Session[\"" , m.Value.Split('.')[1], "\"]"));
                }
            }
            matches = Regex.Matches(Obj, this.Path.tagregex.RequestValue, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Obj = this.Obj.Replace(m.Value, string.Concat("Request.QueryString[\"" , m.Value.Split('.')[1], "\"]"));
                }
            }
            matches = Regex.Matches(Obj, this.Path.tagregex.CookieValue, RegexOptions.IgnoreCase);
            //需放在 RequestValue之后，避免混淆  如Request.Cookies["xxx"]
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Obj = this.Obj.Replace(m.Value, string.Concat("Request.Cookies[\"" , m.Value.Split('.')[1], "\"]"));
                }
            }
            return string.Format("<% ={0} %>", this.Obj);
        }

        public override string ToTagString()
        {
            return "【全局名称" + this.GetTagName() + ",标签类型：Method，全名：" + this.Obj + "，方法："
                + this.Method + "，参数：" + this.Params + "】<br />";
        }

        public void SetDataName(string dataName, MethodType type)
        {
            if (this.Type == type)
            {
                this.ReadDataname = dataName;
            }
        }

        public HashSet<string> GetFieldName()
        {
            var Fields = new HashSet<string>();
            var matches = Regex.Matches(Obj, this.Path.tagregex.ItemValue, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    Fields.Add(m.Value.Split('.')[1]);
                }
            }
            return Fields;
        }
    }
}