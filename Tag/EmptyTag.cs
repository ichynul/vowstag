
using System.Text.RegularExpressions;
using Tag.Vows.Tool;
namespace Tag.Vows.Tag
{
    class EmptyTag : StyleAbleTag
    {
        public EmptyTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {
        }

        protected override string GetCodeForAspx()
        {
            return "";
        }

        protected override void Discover()
        {
        }

        public override string ToTagString()
        {
            string s = "【全局名称" + this.GetTagName() + ",标签类型：EmptyTag，内容：" + 
                Regex.Replace(("" + this.Style).Replace("<", "&lt;").Replace(">", "&gt;"), @"[\r\n\s]", "") + "】<br />";
            return s;
        }
    }
}
