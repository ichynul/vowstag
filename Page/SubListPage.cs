using System.Linq;
using Tag.Vows.Interface;
using Tag.Vows.Enum;
using Tag.Vows.Tag;
using Tag.Vows.Tool;

namespace Tag.Vows.Page
{
    class SubListPage : ItemPage, IUpperDataAble, IUC
    {
        public SubListPage(string mHtmlpPath, string mPageName, int mDeep, TagConfig config)
            : base(mHtmlpPath, mPageName, mDeep, config)
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