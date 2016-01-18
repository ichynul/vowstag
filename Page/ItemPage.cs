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
            return thisIsSimple() ? this : new SubListPage(this.HtmlpPath, this.PageName, this.Deep, this.Path);
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
            Match = Regex.Match(this.Html, this.Path.tagregex.EmptyPairTest, RegexOptions.IgnoreCase);
            string style = "";
            IStyleAble st = null;
            if (Match.Success)
            {
                style = Match.Groups["style"].Value;
                st = this.TagList.FirstOrDefault(x => x is IStyleAble && x.In_Pairs && Match.Value.Contains(x.Text)) as IStyleAble;
                empty = new EmptyTag(Match.Value, Match.Value, Deep, this.Path, this.TagList.Count);
                this.TagList.Add(empty);
                empty.SetStyle(style);
                Html = Html.Replace(Match.Value, string.Empty);
            }
        }

        public string getEmptyText()
        {
            return this.empty == null ? "" : Regex.Replace(this.empty.GetStyle(), "[\r\n]", "");
        }

        private bool thisIsSimple()
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