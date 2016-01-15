using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tag.Vows
{
    class ItemPage : BasePage, IFieldsList
    {
        private EmptyTag empty;
        public ItemPage getItemInstance()
        {
            return thisIsSimple() ? this : new SubListPage(this.HtmlpPath, this.PageName, this.Deep, this.path);
        }

        public ItemPage(string mHtmlpPath, string mPageName, int mDeep, mPaths path)
            : base(mHtmlpPath, mPageName, mDeep, path)
        {
            doForItem();
        }

        public ItemPage(string style, int mDeep, mPaths path)
            : base(style, mDeep, path, "x-item-fake-x")
        {
            doForItem();
        }

        public void doForItem()
        {
            match = Regex.Match(this.Html, this.path.tagregex.EmptyPairTest, RegexOptions.IgnoreCase);
            string style = "";
            IStyleAble st = null;
            if (match.Success)
            {
                style = match.Groups["style"].Value;
                st = this.TagList.FirstOrDefault(x => x is IStyleAble && x.In_Pairs && match.Value.Contains(x.Text)) as IStyleAble;
                empty = new EmptyTag(match.Value, match.Value, Deep, this.path, this.TagList.Count);
                this.TagList.Add(empty);
                empty.SetStyle(style);
                Html = Html.Replace(match.Value, string.Empty);
            }
        }

        public string getEmptyText()
        {
            return this.empty == null ? "" : Regex.Replace(this.empty.getStyle(), "[\r\n]", "");
        }

        private bool thisIsSimple()
        {
            return this.TagList.FirstOrDefault(c => c is ListTag) == null;
        }

        public List<string> GetItemFields()
        {
            List<string> fields = new List<string>();
            IMethodDataAble m = null;
            IFieldDataAble f = null;
            string name = null;
            foreach (var c in this.TagList)
            {
                if (c is IMethodDataAble)
                {
                    m = c as IMethodDataAble;
                    name = m.getFieldName();
                    if (!string.IsNullOrEmpty(name) && !fields.Contains(name.ToLower()))
                    {
                        fields.Add(name.ToLower());
                    }
                }
                else if (c is IFieldDataAble)
                {
                    f = c as IFieldDataAble;
                    name = f.getFieldName();
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