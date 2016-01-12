
namespace Tag.Vows
{
    class ElseIfTag : BaseTag
    {
        private string Test;
        public ElseIfTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
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
            string s = "【全局名称" + this.getTagName() + ",标签类型：ElseIfTag，内容：" + Test + "】<br />";
            return s;
        }
    }
}
