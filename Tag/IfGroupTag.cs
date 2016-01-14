using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Tag.Vows
{
    class IfGroupTag : BaseTag
    {
        private List<IfTag> IfTags;
        protected MatchCollection matches;
        protected Match match;

        public IfGroupTag(string mtext, int Deep, mPaths path, int no_)
            : base(mtext, mtext, Deep, path, no_)
        {
        }
        public override string getCodeForAspx()
        {
            return "";
        }

        protected override void Discover()
        {
            System.Web.HttpContext.Current.Response.Write(this.Text + "----------<br />");
            this.IfTags = new List<IfTag>();
            match = Regex.Match(this.Text, this.path.tagregex.IfTest, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                System.Web.HttpContext.Current.Response.Write("find if :" + match.Value + "<br />");
                this.IfTags.Add(new IfTag(match.Value, IfType._if, this.Deep, this.path, IfTags.Count + 1));
            }
            matches = Regex.Matches(this.Text, this.path.tagregex.ElseIfTest, RegexOptions.IgnoreCase);
            foreach (Match m in matches)
            {
                System.Web.HttpContext.Current.Response.Write("find elseif :" + m.Value + "<br />");
                this.IfTags.Add(new IfTag(m.Value, IfType._else_if, this.Deep, this.path, IfTags.Count + 1));
            }
            match = Regex.Match(this.Text, this.path.tagregex.ElseTest, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                System.Web.HttpContext.Current.Response.Write("find else :" + match.Value + "<br />");
                this.IfTags.Add(new IfTag(match.Value, IfType._else, this.Deep, this.path, IfTags.Count + 1));
            }
        }

        public override string toTagString()
        {
            
            string s = "【全局名称" + this.getTagName() + ",标签类型：IfGroupTag，内容：" + this.Text + "】<br />";
            return s;
        }
    }
}
