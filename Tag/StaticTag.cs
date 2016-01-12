using System;

namespace Tag.Vows
{
    /// <summary>
    ///HeadTag 的摘要说明
    /// </summary>
    class StaticTag : BaseTag, ISubAble
    {
        public string StaticName;
        protected new IHtmlAble SubPage;
        private string ParPageName;

        public StaticTag(string mtext, string mOrigin, int Deep, string mParPageName, mPaths path, int no_)
            : base(mtext,mOrigin, Deep, path, no_)
        {
            this.ParPageName = mParPageName;
        }

        protected override void Discover()
        {
            this.StaticName = this.path.tagregex.getDataName(this.Text);
            if (this.StaticName != null)
            {
                this.LoadSubPage();
            }
            else
            {
                this.Msg += "未指定Static文件名称<br />";
            }
        }

        public override string getCodeForAspx()
        {
            if (ParPageName == this.StaticName)
            {
                return string.Format("<!--（未加载套用自己的static标签）。{0}-->", this.Text);
            }
            return this.SubPage != null ?
                this.SubPage.getAspxCode() :
                string.Format("<!--加载静态页{0}面出错! -->\r\n", this.StaticName);
        }

        public void LoadSubPage()
        {
            this.SubPage = new StaticPage(this.path.StaticlPath, (this as StaticTag).StaticName, Deep, this.path);
            if (path.convert)
            {
                SubPage.ConverterTags();
            }
        }

        public override string toTagString()
        {
            string s = "【全局名称" + this.getTagName() + ",标签类型：static，标签文件名：" + this.StaticName + "】<br />";
            return s;
        }
    }
}