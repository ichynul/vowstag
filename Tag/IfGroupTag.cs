using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Tag.Vows
{
    class IfGroupTag : BaseTag, IMethodDataAble
    {
        private List<IfTag> IfTags;
        protected MatchCollection matches;
        protected Match match;

        public IfGroupTag(string mtext, int Deep, mPaths path, int no_)
            : base(mtext, mtext, Deep, path, no_)
        {

            this.TagName += "xxx";
            this.PlaceholderName += "5555";

        }

        protected override string GetCodeForAspx()
        {
            foreach (var iftag in this.IfTags)
            {
                this.Text = this.Text.Replace(iftag.Text, iftag.GetCode());
            }
            this.Text = Regex.Replace(this.Text, this.Path.tagregex.ifTagKeyTest, string.Empty
                        , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return this.Text;
        }

        protected override void Discover()
        {
            this.IfTags = new List<IfTag>();
            match = Regex.Match(this.Text, this.Path.tagregex.IfTest, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                IfTag iftag = new IfTag(match.Value, IfType._if, this.Deep, this.Path, IfTags.Count + 1);
                iftag.SetTest(match.Groups["test"].Value);
                iftag.SetContent(match.Groups["content"].Value);
                this.IfTags.Add(iftag);
            }
            matches = Regex.Matches(this.Text, this.Path.tagregex.ElseIfTest, RegexOptions.IgnoreCase);
            foreach (Match m in matches)
            {
                IfTag iftag = new IfTag(m.Value, IfType._else_if, this.Deep, this.Path, IfTags.Count + 1);
                iftag.SetTest(match.Groups["test"].Value);
                iftag.SetContent(match.Groups["content"].Value);
                this.IfTags.Add(iftag);
            }
            match = Regex.Match(this.Text, this.Path.tagregex.ElseTest, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                IfTag iftag = new IfTag(match.Value, IfType._else, this.Deep, this.Path, IfTags.Count + 1);
                iftag.SetTest(match.Groups["test"].Value);
                iftag.SetContent(match.Groups["content"].Value);
                this.IfTags.Add(iftag);
            }
        }

        public override string ToTagString()
        {
            string s = "【全局名称" + this.GetTagName() + ",标签类型：IfGroupTag，内容：" + this.Text + "】<br />";
            return s;
        }

        public void SetDataName(string dataName, MethodType type)
        {
            throw new System.NotImplementedException();
        }

        public HashSet<string> GetFieldName()
        {
            throw new System.NotImplementedException();
        }
    }
}
