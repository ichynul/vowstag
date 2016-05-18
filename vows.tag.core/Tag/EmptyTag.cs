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
using System.Text.RegularExpressions;
using Tag.Vows.Tool;
using Tag.Vows.Interface;
using Tag.Vows.Page;

namespace Tag.Vows.Tag
{
    class EmptyTag : BaseTag
    {
        public const string FakeNameStr = "-x-empty-fake-x-";

        public EmptyTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {
        }

        protected override string GetCodeForAspx()
        {
            return string.Empty;
        }

        protected override void Discover()
        {

        }

        public override string ToTagString()
        {
            string s = "【全局名称" + this.GetTagName() + ",标签类型：EmptyTag，内容：" +
                Regex.Replace(("" + this.Text).Replace("<", "&lt;").Replace(">", "&gt;"), @"[\r\n\s]", "") + "】<br />";
            return s;
        }

        public string FindEmptyContent(string style, out string emptyText)
        {
            emptyText = "";
            Match m = Regex.Match(style, string.Concat(this.PlaceHolderName,
                    @"(?<content>.*?)", this.PlaceHolderName),
                    RegexOptions.Singleline);
            if (m.Success)
            {
                style = style.Replace(m.Value, string.Empty);
                emptyText = m.Groups["content"].Value;
            }
            return style;
        }

        public string GetContentPlaceholder(string style)
        {
            return string.Concat(this.PlaceHolderName, style, this.PlaceHolderName);
        }
    }
}
