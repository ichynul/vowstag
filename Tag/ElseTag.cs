
namespace Tag.Vows
{
    class ElseTag : BaseTag
    {
        private string Test;
        public ElseTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {

        }
        public override string getCodeForAspx()
        {
            return string.Format("<!-- {0} -->", this.tagName);
        }

        protected override void Discover()
        {
        }

        public override string toTagString()
        {
            string s = "【全局名称" + this.getTagName() + ",标签类型：ElseTag，内容：" + Test + "】<br />";
            return s;
        }
    }
}
