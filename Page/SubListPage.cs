using System.Linq;

namespace Tag.Vows
{
    class SubListPage : ItemPage, IParentDataAble, IUC
    {
        public SubListPage(string mHtmlpPath, string mPageName, int mDeep, mPaths path)
            : base(mHtmlpPath, mPageName, mDeep, path)
        {

        }

        public void SetUpperDataName(string DataName, FieldType type)
        {
            foreach (var c in this.TagList)
            {
                if (c is ListTag)
                {
                    (c as ListTag).SetUpperDataName(DataName, FieldType.item_value);
                }
                else if (c is FieldTag)
                {
                    (c as FieldTag).SetDataName(DataName, FieldType.item_value);
                }
            }
        }
    }
}