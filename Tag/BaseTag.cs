/*
* The MIT License (MIT)

*Copyright (c) 2015 ichynul

*Permission is hereby granted, free of charge, to any person obtaining a copy
*of this software and associated documentation files (the "Software"), to deal
*in the Software without restriction, including without limitation the rights
*to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*copies of the Software, and to permit persons to whom the Software is
*furnished to do so, subject to the following conditions:

*The above copyright notice and this permission notice shall be included in all
*copies or substantial portions of the Software.

*THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
*LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
*OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
*SOFTWARE.
*/

using System.Text.RegularExpressions;
using Tag.Vows.Interface;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    /// <summary>
    ///2015/07/15
    ///by lianghaiyun
    /// </summary>
    abstract class BaseTag : IConvertAble
    {
        public int Sort = 0;
        protected int NO_ = 0;
        protected TagConfig Config;
        protected string TagName;
        protected string PlaceholderName;
        public bool In_Pairs { get; private set; }
        public string Text { get; protected set; }
        public string Origin { get; protected set; }
        protected string Msg = "";
        internal IMakeAble SubPage = null;
        protected int Deep;
        protected BaseTag() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mText">经过处理的标签文本（去除空格 \s*、br/hr替换）</param>
        /// <param name="mOrigin">未经过处理的标签原始文本</param>
        /// <param name="mDeep"></param>
        /// <param name="path"></param>
        /// <param name="no_"></param>
        protected BaseTag(string mText, string mOrigin, int mDeep, TagConfig config, int no_)
        {
            this.Config = config;
            this.Deep = mDeep;
            this.Text = mText;
            this.Origin = mOrigin;
            this.NO_ = no_;
            this.TagName = string.Concat(this.GetType().Name, "_", this.Text.Length, "_", (this.NO_ + 1));
            this.PlaceholderName = string.Concat(this.GetType().Name, this.Text.Length, (this.NO_ + 1));
            this.In_Pairs = this.Text.LastIndexOf('/') != this.Text.Length - 2;
            this.Discover();
        }

        /// <summary>
        /// 将标签文本替换成名称...{tagtext...} => tagname
        /// </summary>
        /// <param name="PageHtml"></param>
        /// <returns></returns>
        public string ReplaceTagText(string PageHtml)
        {
            System.Web.HttpContext.Current.Response.Write("rp" + PlaceholderName + "<br />");

            string newHtml = PageHtml.Replace(this.Text, this.PlaceholderName);
            return newHtml;
        }

        /// <summary>
        /// 将标签成名称替换为经过处理的文本... tagname => tagcode
        /// </summary>
        /// <param name="PageHtml"></param>
        /// <returns></returns>
        public string RecoverTagText(string PageHtml)
        {
            System.Web.HttpContext.Current.Response.Write("rc" + PlaceholderName + "<br />");
            string newHtml = PageHtml.Replace(this.PlaceholderName, this.GetCodeForAspx());
            return newHtml;
        }

        /// <summary>
        /// 当执行标签对转换时调用
        /// </summary>
        /// <returns></returns>
        public string ConvertTagPair()
        {
            if (Config.convert_pairs != null || Config.convert_pairs.Length == 2)
            {
                this.Origin = Regex.Replace(this.Origin, "^" + Config.tagLeft, Config.convert_pairs[0]);
                this.Origin = Regex.Replace(this.Origin, Config.tagRight + "$", Config.convert_pairs[1]);
            }
            return this.Origin;
        }

        public string GetTagName()
        {
            return this.TagName;
        }

        public string GetPlaceholderName()
        {
            return this.PlaceholderName;
        }

        public string GetMsg()
        {
            if (this.SubPage != null)
            {
                this.Msg += SubPage.GetMsg();
            }
            return this.Msg;
        }
        /// <summary>
        /// 子类实现将标签转换为代码的具体逻辑
        /// </summary>
        /// <returns></returns>
        protected abstract string GetCodeForAspx();
        /// <summary>
        /// 子类实现一些初始化工作
        /// </summary>
        protected abstract void Discover();
        /// <summary>
        /// 输出标签的内容，以供调试
        /// </summary>
        /// <returns></returns>
        public abstract string ToTagString();
    }
}