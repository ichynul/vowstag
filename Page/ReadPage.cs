using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tag.Vows
{
    class ReadPage : BasePage, IParentDataAble
    {
        public ReadPage(string mHtmlpPath, string mPageName, int mDeep, mPaths path)
            : base(mHtmlpPath, mPageName, mDeep, path)
        {

        }

        public ReadPage(string style, int mDeep, mPaths path)
            : base(style, mDeep, path, "x-read-fake-x")
        {
        }

        public void setUpDataName(string DataName, FieldType type)
        {
            foreach (var c in TagList)
            {
                if (c is FieldTag)
                {
                    (c as FieldTag).setDataName(DataName, FieldType.readValue);
                }
                else if (c is MethodTag)
                {
                    (c as MethodTag).setDataName(DataName, MethodType.readValue);
                }
            }
        }

        public List<Method> getListMethods(string DataName)
        {
            List<Method> listMethods = new List<Method>();
            StringBuilder sb = new StringBuilder();
            ListTag list = null;
            foreach (var x in this.TagList.Where(x => x is ListTag))
            {
                list = x as ListTag;
                list.setUpDataName(DataName, FieldType.readValue);
                list.set_Inside_Read();
                Method m = list.getGloabalMethod();
                if (m != null)
                {
                    listMethods.Add(m);
                }
            }
            return listMethods;
        }
    }
}
