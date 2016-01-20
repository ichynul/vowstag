using System.Collections.Generic;
using Tag.Vows.Interface;
using Tag.Vows.Enum;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class IfTag : BaseTag, ITestAble, IMethodDataAble
    {
        private string Test;
        private string Conttent;
        private IfType Type;
        private string DataName;

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
            this.DataName = dataName;
        }

        public HashSet<string> GetFieldName()
        {
            throw new System.NotImplementedException();
        }
    }
}
