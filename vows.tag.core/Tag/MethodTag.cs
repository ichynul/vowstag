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
        private string Dataname = "";
        public MethodType Type { get; private set; }
        public string forname { get; set; }
        public MethodTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {
        }

        protected override void Discover()
        {
            this.Obj = this.Config.tagregex.getMethodObj(this.Text);
            string[] arr = Obj.Split('(');
            this.Method = arr[0];
            this.Params = arr[1].Replace(")", string.Empty);
            if (this.Config.tagregex.ReadValue.IsMatch(Obj))
            {
                this.Type = MethodType.read_value_method;
            }
            else if (this.Config.tagregex.ItemValue.IsMatch(Obj))
            {
                this.Type = MethodType.item_value_method;
            }
            else if (this.Config.tagregex.FormValue.IsMatch(Obj))
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
                return string.Format("_tagcall.form('{0}',{1}); return false;", this.forname, this.Params.ToLower() == "false" ? "false" : "true");
            }
            if (!string.IsNullOrEmpty(this.Dataname) && this.Type == MethodType.read_value_method)
            {
                var resdMatches = this.Config.tagregex.ReadValue.Matches(Obj);
                if (resdMatches.Count > 0)
                {
                    Dataname = Helper.GetTableName(Dataname);
                    string readField = "";
                    foreach (Match m in resdMatches)
                    {
                        readField = Helper.GetModFieldName(Dataname, m.Value.Split('.')[1]);
                        if (!string.IsNullOrEmpty(readField))
                        {
                            this.Obj = this.Obj.Replace(m.Value, string.Concat("read.", readField));
                        }
                    }
                }
            }
            var matches = this.Config.tagregex.ItemValue.Matches(Obj);
            bool hasItem = false;
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    if (!string.IsNullOrEmpty(this.Dataname))//sublist 中
                    {
                        string itemField = m.Value.Split('.')[1];
                        itemField = Helper.GetModFieldName(Dataname, itemField);
                        this.Obj = this.Obj.Replace(m.Value, string.Concat("item." + itemField));
                    }
                    else
                    {
                        hasItem = true;
                        this.Obj = this.Obj.Replace(m.Value, string.Concat("Eval(\"", m.Value.Split('.')[1], "\")"));
                    }
                }
            }
            matches = this.Config.tagregex.SessionValue.Matches(Obj);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Obj = this.Obj.Replace(m.Value, string.Concat("Session[\"", m.Value.Split('.')[1], "\"]"));
                }
            }
            matches = this.Config.tagregex.RequestValue.Matches(Obj);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Obj = this.Obj.Replace(m.Value, string.Concat("Request.QueryString[\"", m.Value.Split('.')[1], "\"]"));
                }
            }
            matches = this.Config.tagregex.CookieValue.Matches(Obj);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    this.Obj = this.Obj.Replace(m.Value, string.Concat("Request.Cookies[\"", m.Value.Split('.')[1], "\"]"));
                }
            }
            if (hasItem)
            {
                return string.Format("<%# {0} %>", this.Obj);
            }
            else
            {
                return string.Format("<% = {0} %>", this.Obj);
            }
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
                this.Dataname = dataName;
            }
        }

        public HashSet<string> GetItemFieldNames(string tableName)
        {
            var Fields = new HashSet<string>();
            var matches = this.Config.tagregex.ItemValue.Matches(this.Obj);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    string name = this.Helper.GetModFieldName(tableName, m.Value.Split('.')[1]);
                    this.Obj = this.Obj.Replace(m.Value, string.Concat("item.", name));
                    Fields.Add(name);
                }
            }
            return Fields;
        }
    }
}