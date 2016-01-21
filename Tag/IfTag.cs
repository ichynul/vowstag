using System.Collections.Generic;
using Tag.Vows.Interface;
using Tag.Vows.Enum;
using Tag.Vows.Tool;
using System.Text.RegularExpressions;
using Tag.Vows.Data;

namespace Tag.Vows.Tag
{
    class IfTag : BaseTag, ITestAble, IMethodDataAble
    {
        private string Test;
        private string Conttent;
        private IfType Type;
        private string ReadDataname;

        public IfTag(string mtext, IfType type, int Deep, TagConfig config, int no_)
            : base(mtext, mtext, Deep, config, no_)
        {
            this.Type = type;
        }

        protected override string GetCodeForAspx()
        {
            return string.Empty;
        }

        protected override void Discover()
        {
        }

        public override string ToTagString()
        {
            string s = "【全局名称" + this.GetTagName() + ",标签类型：" + this.Type + "，内容：if(" + this.Test + "){" + this.Conttent + "}】<br />";
            return s;
        }

        public string GetCode()
        {
            this.CheckTestContent();

            if (this.Type == IfType._if)
            {
                return string.Concat("<% if (", this.Test, ") %>\r\n<% { %>", this.Conttent, "<% } %>\r\n");
            }
            else if (this.Type == IfType._else_if)
            {
                return string.Concat("<% else if (", this.Test, ") %>\r\n<% { %>", this.Conttent, "<% } %>\r\n");
            }
            else
            {
                return string.Concat("<% else %>\r\n<% { %>", this.Conttent, "<% } %>\r\n");
            }
        }

        public void SetTest(string test)
        {
            this.Test = test;
        }

        public void SetContent(string content)
        {
            this.Conttent = content;
        }

        public void SetDataName(string dataName, MethodType type)
        {
            this.ReadDataname = dataName;
        }

        public HashSet<string> GetFieldName()
        {
            var Fields = new HashSet<string>();
            var matches = Regex.Matches(this.Test, this.Config.tagregex.ItemValue, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    Fields.Add(m.Value.Split('.')[1]);
                }
            }
            return Fields;
        }

        public void CheckTestContent()
        {
            if (!string.IsNullOrEmpty(this.ReadDataname) &&
                Regex.IsMatch(this.Test, this.Config.tagregex.ReadValue, RegexOptions.IgnoreCase))
            {
                var resdMatches = Regex.Matches(this.Test, this.Config.tagregex.ReadValue, RegexOptions.IgnoreCase);
                if (resdMatches.Count > 0)
                {
                    ReadDataname = TempleHelper.getTempleHelper(this.Config).GetTableName(ReadDataname);
                    string itemField = "";
                    foreach (Match m in resdMatches)
                    {
                        itemField = TempleHelper.getTempleHelper(this.Config).GetModFieldName(ReadDataname, m.Value.Split('.')[1]);
                        if (!string.IsNullOrEmpty(itemField))
                        {
                            this.Test = this.Test.Replace(m.Value, string.Concat("read.", itemField));
                        }
                    }
                }
            }
            var matches = Regex.Matches(this.Test, this.Config.tagregex.ItemValue, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Test = this.Test.Replace(m.Value, string.Concat("<%# Eval(\"", m.Value.Split('.')[1], "\") %>"));
                }
            }
            matches = Regex.Matches(this.Test, this.Config.tagregex.SessionValue, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Test = this.Test.Replace(m.Value, string.Concat("Session[\"", m.Value.Split('.')[1], "\"]"));
                }
            }
            matches = Regex.Matches(this.Test, this.Config.tagregex.RequestValue, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Test = this.Test.Replace(m.Value, string.Concat("Request.QueryString[\"", m.Value.Split('.')[1], "\"]"));
                }
            }
            matches = Regex.Matches(this.Test, this.Config.tagregex.CookieValue, RegexOptions.IgnoreCase);
            //需放在 RequestValue之后，避免混淆  如Request.Cookies["xxx"]
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Test = this.Test.Replace(m.Value, string.Concat("Request.Cookies[\"", m.Value.Split('.')[1], "\"]"));
                }
            }
            this.Test = Regex.Replace(this.Test, @"={1}", "==");
            this.Test = Regex.Replace(this.Test, @"&{1}", "&&");
            this.Test = Regex.Replace(this.Test, @"\|{1}", "||");
        }
    }
}
