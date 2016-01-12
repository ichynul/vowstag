using System.Linq;

namespace Tag.Vows
{
    class SubListPage : ItemPage, IParentDataAble, IUC
    {
        public SubListPage(string mHtmlpPath, string mPageName, int mDeep, mPaths path)
            : base(mHtmlpPath, mPageName, mDeep, path)
        {

        }

        public void setUpDataName(string DataName, FieldType type)
        {
            var tages = TagList;
            foreach (var c in tages)
            {
                if (c is ListTag)
                {
                    (c as ListTag).setUpDataName(DataName, FieldType.itemValue);
                }
                else if (c is FieldTag)
                {
                    (c as FieldTag).setDataName(DataName, FieldType.itemValue);
                }
                else if (c is MethodTag)
                {
                    (c as MethodTag).setDataName(DataName, MethodType.itemValue);
                }
            }
        }
    }
}