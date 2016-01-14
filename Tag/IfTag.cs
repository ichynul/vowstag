
namespace Tag.Vows
{
    class IfTag : BaseTag, ITestAble
    {
        private string Test;
        private string Conttent;
        private IfType Type;

        public IfTag(string mtext, IfType type, int Deep, mPaths path, int no_)
            : base(mtext, mtext, Deep, path, no_)
        {
            this.Type = type;
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
            string s = "【全局名称" + this.getTagName() + ",标签类型：" + this.Type + "，内容：if(" + this.Test + "){" + this.Conttent + "}】<br />";
            return s;
        }

        public void SetTest(string test)
        {
            this.Test = test;
        }

        public void SetContent(string content)
        {
            this.Conttent = content;
        }
    }
}
