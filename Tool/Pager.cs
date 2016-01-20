using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tag.Vows.Tool
{
    /// <summary>
    /// 2015/07/25
    /// by liagnhaiyun
    /// </summary>
    public class Pager
    {
        protected int ListSize;
        protected int Current_page;
        protected int Num_display = 10;
        protected int PageSize;
        protected int Num_edge = 3;
        protected string Link_to = "";
        protected string Prev_text = "上一页";
        protected string Next_text = "下一页";
        protected string Ellipse_text = "...";
        protected bool PrevOrNext_show = true;

        protected int TotalPage;

        protected StringBuilder htmlLinks;

        public Pager(int mListSize, int mCunrrent, int mPageSize, string mLink, int mNum_edge, string mPrev_text, string mNext_text, string mEllipse_text, bool mPrevOrNext_show)
        {
            this.ListSize = mListSize;
            this.Current_page = mCunrrent;
            this.PageSize = mPageSize;
            this.Link_to = Regex.Replace(mLink, @"&?page=[^&]*(?=&|$)", string.Empty, RegexOptions.IgnoreCase); ;
            if (Link_to.IndexOf("?") == -1)
            {
                this.Link_to += "?";
            }
            else if (!Link_to.EndsWith("?") && !Link_to.EndsWith("&"))
            {
                this.Link_to += "&";
            }
            //
            this.Num_edge = mNum_edge;
            this.Prev_text = mPrev_text;
            this.Next_text = mNext_text;
            this.Ellipse_text = mEllipse_text;
            this.PrevOrNext_show = mPrevOrNext_show;

            GaculTotalPages();
        }
        public void SetPagesize(int psize)
        {
            this.PageSize = psize;
        }

        protected void GaculTotalPages()
        {
            TotalPage = ListSize / PageSize;
            if (ListSize % PageSize != 0)
            {
                TotalPage += 1;
            }
            if (TotalPage < 1)
            {
                TotalPage = 1;
            }
            Current_page = Current_page > TotalPage ? TotalPage : Current_page < 1 ? 1 : Current_page;
            Num_edge = Num_edge > TotalPage ? TotalPage : Num_edge;
            Num_edge = Num_edge > Num_display ? Num_display / 2 : Num_edge;
            Num_edge = Num_edge < 1 ? 1 : Num_edge;
        }

        public string MakeLinks()
        {
            htmlLinks = new StringBuilder();
            if (PrevOrNext_show && Current_page > 1)
            {
                htmlLinks.AppendFormat("<a class='prev' href='{0}page={1}'>{2}</a>", Link_to, Current_page - 1, Prev_text);
            }
            else
            {
                htmlLinks.AppendFormat("<a class='disabled'>{0}</a>", Prev_text);
            }
            List<int> drawed = new List<int>();

            for (int i = 1; i <= Num_edge; i += 1)
            {
                if (i != Current_page)
                {
                    htmlLinks.AppendFormat("<a href='{0}page={1}'>{1}</a>", Link_to, i);
                }
                else
                {
                    htmlLinks.AppendFormat("<span class='current'>{0}</span>", i);
                }
                drawed.Add(i);
            }
            int half = Num_display / 2;
            int start = Current_page - half;
            int end = Current_page + half;

            if (start < 1)
            {
                end += Math.Abs(0 - start) + Num_edge - 2;
                start = 1;
            }

            if (end > TotalPage)
            {
                start -= (end - TotalPage) - 1;
                end = TotalPage;
            }

            if (start < 1)
            {
                start = 1;
            }

            if (end > TotalPage)
            {
                end = TotalPage;
            }

            if (start > Num_edge + 1)
            {
                htmlLinks.AppendFormat("<span>{0}</span>", Ellipse_text);
            }

            for (int i = start; i < end; i += 1)
            {
                if (drawed.Contains(i))
                {
                    continue;
                }
                if (i != Current_page)
                {
                    htmlLinks.AppendFormat("<a href='{0}page={1}'>{1}</a>", Link_to, i);
                }
                else
                {
                    htmlLinks.AppendFormat("<span class='current'>{0}</span>", i);
                }
                drawed.Add(i);
            }

            if (end < TotalPage - Num_edge - 1)
            {
                htmlLinks.AppendFormat("<span>{0}</span>", Ellipse_text);
            }

            for (int i = TotalPage - Num_edge + 1; i <= TotalPage; i += 1)
            {
                if (drawed.Contains(i) || i < 1)
                {
                    continue;
                }
                if (i != Current_page)
                {
                    htmlLinks.AppendFormat("<a href='{0}page={1}'>{1}</a>", Link_to, i);
                }
                else
                {
                    htmlLinks.AppendFormat("<span class='current'>{0}</span>", i);
                }
            }

            if (PrevOrNext_show && Current_page < TotalPage)
            {
                htmlLinks.AppendFormat("<a class='prev' href='{0}page={1}'>{2}</a>", Link_to, Current_page + 1, Next_text);
            }
            else
            {
                htmlLinks.AppendFormat("<a class='disabled'>{0}</a>", Next_text);
            }
            return htmlLinks.ToString();
        }
    }
}
