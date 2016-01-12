using System.Collections.Generic;
namespace Tag.Vows
{
    class IfGroupTag : StyleAbleTag
    {
        private IfTag If;
        private List<ElseIfTag> Elses;
        private ElseTag Else;
        protected new IHtmlAble SubPage;

        public IfGroupTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {

        }
        public override string getCodeForAspx()
        {
            System.Web.HttpContext.Current.Response.Write(this.Text + ":" + this.Style + "------------<br />");
            return "";
        }

        protected override void Discover()
        {
            
        }

        public override string toTagString()
        {
            string s = "【全局名称" + this.getTagName() + ",标签类型：IfGroupTag，内容：" + this.Text + "】<br />";
            return s;
        }
    }
}
