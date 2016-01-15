
namespace Tag.Vows
{
    class PagerTag : BaseTag
    {
        public PagerTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {

        }

        protected string PageParams;

        protected bool Used;

        internal PagerType type = PagerType.cs;
        public int Num_edge = 3;
        public int Num_display = 10;
        public string Prev_text = "上一页";
        public string Next_text = "下一页";
        public string Ellipse_text = "...";
        public bool PrevOrNext_show = true;

        public void setUsed(bool mUsed)
        {
            this.Used = mUsed;
        }

        public bool IsUsed()
        {
            return Used;
        }
        protected override string getCodeForAspx()
        {
            return string.Format("<asp:Literal ID=\"{0}\" runat=\"server\"></asp:Literal>", this.getTagName());
        }

        protected override void Discover()
        {
            if (this.Text.IndexOf('?') != -1)
            {
                this.PageParams = this.path.tagregex.getBaseParams(this.Text);
                if (!string.IsNullOrEmpty(PageParams))
                {
                    string[] arr = PageParams.Split('&');
                    string[] pm = null;
                    string name = null;
                    string value = null;
                    foreach (string s in arr)
                    {
                        if (s.IndexOf('=') != -1)
                        {
                            pm = s.Split('=');
                            if (pm.Length > 1)
                            {
                                name = pm[0].ToLower();
                                value = pm[1];
                                if (string.IsNullOrEmpty(value))
                                {
                                    continue;
                                }
                                if (name == "type" && value == "js")
                                {
                                    this.type = PagerType.js;
                                }
                                else if (name == "edge")
                                {
                                    int.TryParse(value, out Num_edge);
                                }
                                else if (name == "next")
                                {
                                    Next_text = value;
                                }
                                else if (name == "num_display")
                                {
                                    int.TryParse(name, out Num_display);
                                }
                                else if (name == "prev")
                                {
                                    Prev_text = value;
                                }
                                else if (name == "next_prev_show")
                                {
                                    PrevOrNext_show = value == "1" || value == "true" || value == "yes";
                                }
                                else if (name == "ellipse")
                                {
                                    Ellipse_text = value;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string toTagString()
        {
            return "【全局名称：" + this.getTagName() + ",标签类型：pager" + "，分页方式：" +
                (this.type == PagerType.js ? "js" : "cs") + "，首尾显示：" + this.Num_edge + "条，显示上下翻页："
                + this.PrevOrNext_show + "，next文字：" + this.Next_text + "，prev文字："
                + this.Prev_text + "，省略符号：" + this.Ellipse_text + "】<br />";
        }
    }
}
