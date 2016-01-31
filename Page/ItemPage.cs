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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tag.Vows.Interface;
using Tag.Vows.Tag;
using Tag.Vows.Tool;

namespace Tag.Vows.Page
{
    class ItemPage : BasePage, IFieldsList
    {
        private EmptyTag Empty;
        public ItemPage GetItemInstance()
        {
            return ThisIsSimple() ? this : new SubListPage(this.HtmlpPath, this.PageName, this.Deep, this.Config);
        }

        public ItemPage(string mHtmlpPath, string mPageName, int mDeep, TagConfig config)
            : base(mHtmlpPath, mPageName, mDeep, config)
        {
            DoForItem();
        }

        public ItemPage(string style, int mDeep, TagConfig config, string listTagName)
            : base(style, mDeep, config, "x-item-fake-x")
        {
            this.PageName = string.Concat(listTagName, "#", this.PageName);
            DoForItem();
        }

        public void DoForItem()
        {
            Match = this.Config.tagregex.EmptyPairTest.Match(this.Html);
            string style = "";
            IStyleAble st = null;
            if (Match.Success)
            {
                style = Match.Groups["style"].Value;
                st = this.TagList.FirstOrDefault(x => x is IStyleAble && x.In_Pairs && Match.Value.Contains(x.Text)) as IStyleAble;
                Empty = new EmptyTag(Match.Value, Match.Value, Deep, this.Config, this.TagList.Count);
                this.TagList.Add(Empty);
                Empty.SetStyle(style);
                Html = Html.Replace(Match.Value, string.Empty);
            }
        }

        public string GetEmptyText()
        {
            return this.Empty == null ? "" : Regex.Replace(this.Empty.GetStyle(), "[\r\n]", "");
        }

        private bool ThisIsSimple()
        {
            return this.TagList.FirstOrDefault(c => c is ListTag) == null;
        }

        public HashSet<string> GetItemFields()
        {
            var fields = new HashSet<string>();
            IMethodDataAble m = null;
            IFieldDataAble f = null;
            string name = null;
            var names = new HashSet<string>();
            foreach (var c in this.TagList)
            {
                if (c is IMethodDataAble)
                {
                    m = c as IMethodDataAble;
                    names = m.GetFieldName();
                    foreach (var fname in names)
                    {
                        fields.Add(fname.ToLower());
                    }
                }
                else if (c is IFieldDataAble)
                {
                    f = c as IFieldDataAble;
                    name = f.GetFieldName();
                    if (!string.IsNullOrEmpty(name) && !fields.Contains(name.ToLower()))
                    {
                        fields.Add(name.ToLower());
                    }
                }
            }

            return fields;
        }
    }
}