#region  The MIT License (MIT)
/*
The MIT License (MIT)

Copyright (c) 2015 ichynul

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
using Tag.Vows.Enum;
using Tag.Vows.Tool;
namespace Tag.Vows.Tag
{
    class PagerTag : BaseTag
    {
        public PagerTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {

        }

        protected string PageParams;

        internal PagerType type = PagerType.cs;
        public int Num_edge = 3;
        public int Num_display = 10;
        public string Prev_text = "上一页";
        public string Next_text = "下一页";
        public string Dom = "div";
        public string Dom_ID = "pager";
        public string Dom_Class = "mypager";
        public string Ellipse_text = "...";
        public bool PrevOrNext_show = true;

        protected override string GetCodeForAspx()
        {
            return string.Format("<asp:Literal ID=\"{0}\" runat=\"server\"></asp:Literal>", this.GetTagName());
        }

        protected override void Discover()
        {
            if (this.Text.IndexOf('?') != -1)
            {
                this.PageParams = this.Config.tagregex.getBaseParams(this.Text);
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
                                else if (name == "dom")
                                {
                                    Dom = value;
                                }
                                else if (name == "domid")
                                {
                                    Dom_ID = value;
                                }
                                else if (name == "domclass")
                                {
                                    Dom_Class = value;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string ToTagString()
        {
            return "【全局名称：" + this.GetTagName() + ",标签类型：pager" + "，分页方式：" +
                (this.type == PagerType.js ? "js" : "cs") + "，首尾显示：" + this.Num_edge + "条，显示上下翻页："
                + this.PrevOrNext_show + "，next文字：" + this.Next_text + "，prev文字："
                + this.Prev_text + "，省略符号：" + this.Ellipse_text + "】<br />";
        }
    }
}
