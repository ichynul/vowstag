
using System.Text.RegularExpressions;
namespace Tag.Vows
{
    class EmptyTag : StyleAbleTag
    {
        public EmptyTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
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
