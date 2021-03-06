﻿#region  The MIT License (MIT)
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
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tag.Vows.Web
{
    /// <summary>
    /// 2015/07/25
    /// by liagnhaiyun
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// list总记录数
        /// </summary>
        protected int ListSize;
        /// <summary>
        /// 当前页码
        /// </summary>
        protected int Current_page;
        /// <summary>
        /// 显示页码数
        /// </summary>
        protected int Num_display = 10;
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        protected int PageSize;
        /// <summary>
        /// 左右两边显示的页码数
        /// </summary>
        protected int Num_edge = 3;
        /// <summary>
        /// 跳转链接
        /// </summary>
        protected string Link_to = "";
        /// <summary>
        /// 上一页的文本
        /// </summary>
        protected string Prev_text = "上一页";
        /// <summary>
        /// 下一页的文本
        /// </summary>
        protected string Next_text = "下一页";
        /// <summary>
        /// 省略号...
        /// </summary>
        protected string Ellipse_text = "...";
        /// <summary>
        /// 始终显示 上一页、下一页
        /// </summary>
        protected bool PrevOrNext_show = true;
        /// <summary>
        /// 总页数
        /// </summary>
        protected int TotalPage;

        /// <summary>
        /// 生成的html
        /// </summary>
        protected StringBuilder htmlLinks;

        /// <summary>
        /// 构造函数（基本参数）
        /// </summary>
        /// <param name="mListSize">list总记录数</param>
        /// <param name="mCunrrent">当前页码</param>
        /// <param name="mPageSize">每页显示记录数</param>
        /// <param name="mLink">跳转链接</param>
        public Pager(int mListSize, int mCunrrent, int mPageSize, string mLink)
        {
            this.ListSize = mListSize;
            this.Current_page = mCunrrent;
            this.PageSize = mPageSize;
            this.Link_to = Regex.Replace(mLink, @"(?<=\?|&)page=[^&]*?(?=&|$)", string.Empty, RegexOptions.IgnoreCase);
            if (Link_to.IndexOf("?") == -1)
            {
                this.Link_to += "?";
            }
            else if (!Link_to.EndsWith("?") && !Link_to.EndsWith("&"))
            {
                this.Link_to += "&";
            }
            this.Link_to = Regex.Replace(this.Link_to, @"&{2,}", "&", RegexOptions.IgnoreCase);
            this.GaculTotalPages();
        }

        /// <summary>
        /// 构造函数（详细参数）
        /// </summary>
        /// <param name="mListSize">list总记录数</param>
        /// <param name="mCunrrent">当前页码</param>
        /// <param name="mPageSize">每页显示记录数</param>
        /// <param name="mLink">跳转链接</param>
        /// <param name="mNum_edge">左右两边显示的页码数</param>
        /// <param name="mPrev_text">上一页的文本</param>
        /// <param name="mNext_text">下一页的文本</param>
        /// <param name="mEllipse_text">省略号...</param>
        /// <param name="mPrevOrNext_show">始终显示 上一页、下一页</param>
        public Pager(int mListSize, int mCunrrent, int mPageSize, string mLink, int mNum_edge, string mPrev_text, string mNext_text, string mEllipse_text, bool mPrevOrNext_show)
        {
            this.ListSize = mListSize;
            this.Current_page = mCunrrent;
            this.PageSize = mPageSize;
            this.Link_to = Regex.Replace(mLink, @"(?<=\?|&)page=[^&]*?(?=&|$)", string.Empty, RegexOptions.IgnoreCase); ;
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
            this.GaculTotalPages();
        }

        /// <summary>
        /// 计算总共有几页
        /// </summary>
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

        /// <summary>
        /// 生成html
        /// </summary>
        /// <returns>html</returns>
        public string MakeLinks()
        {
            htmlLinks = new StringBuilder();
            if (PrevOrNext_show)
            {
                if (Current_page > 1)
                {
                    htmlLinks.AppendFormat("<a class='prev' href='{0}page={1}'>{2}</a>", Link_to, Current_page - 1, Prev_text);
                }
                else
                {
                    htmlLinks.AppendFormat("<a class='disabled'>{0}</a>", Prev_text);
                }
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
            if (PrevOrNext_show)
            {
                if (Current_page < TotalPage)
                {
                    htmlLinks.AppendFormat("<a class='next' href='{0}page={1}'>{2}</a>", Link_to, Current_page + 1, Next_text);
                }
                else
                {
                    htmlLinks.AppendFormat("<a class='disabled'>{0}</a>", Next_text);
                }
            }
            return htmlLinks.ToString();
        }
    }
}
