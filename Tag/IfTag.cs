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
            var matches = this.Config.tagregex.ItemValue.Matches(this.Test);
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
                this.Config.tagregex.ReadValue.IsMatch(this.Test))
            {
                var resdMatches = this.Config.tagregex.ReadValue.Matches(this.Test);
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
            var matches = this.Config.tagregex.ItemValue.Matches(this.Test);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Test = this.Test.Replace(m.Value, string.Concat("<%# ValueOf(Eval(\"", m.Value.Split('.')[1], "\")) %>"));
                }
            }
            matches = this.Config.tagregex.SessionValue.Matches(this.Test);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Test = this.Test.Replace(m.Value, string.Concat("Session[\"", m.Value.Split('.')[1], "\"]"));
                }
            }
            matches = this.Config.tagregex.RequestValue.Matches(this.Test);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Test = this.Test.Replace(m.Value, string.Concat("Request.QueryString[\"", m.Value.Split('.')[1], "\"]"));
                }
            }
            matches = this.Config.tagregex.CookieValue.Matches(this.Test);
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
