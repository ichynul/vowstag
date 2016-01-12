
namespace Tag.Vows
{
    class IfTag : StyleAbleTag
    {
        private string Test;

        public IfTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
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
            string s = "【全局名称" + this.getTagName() + ",标签类型：IfTag，内容：" + this.Text + this.Style + "】<br />";
            return s;
        }
    }
}
