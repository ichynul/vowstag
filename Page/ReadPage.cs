using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tag.Vows.Interface;
using Tag.Vows.Enum;
using Tag.Vows.Web;
using Tag.Vows.Tag;
using Tag.Vows.Tool;

namespace Tag.Vows.Page
{
    class ReadPage : BasePage, IUpperDataAble
    {
        public ReadPage(string mHtmlpPath, string mPageName, int mDeep, TagConfig config)
            : base(mHtmlpPath, mPageName, mDeep, config)
        {

        }

        public ReadPage(string style, int mDeep, TagConfig config)
            : base(style, mDeep, config, "x-read-fake-x")
        {
        }

        public void SetUpperDataName(string dataName, FieldType type)
        {
            foreach (var c in TagList)
            {
                if (c is IFieldDataAble)
                {
                    (c as IFieldDataAble).SetDataName(dataName, FieldType.read_value);
                }
                else if (c is IMethodDataAble)
                {
                    (c as IMethodDataAble).SetDataName(dataName, MethodType.read_value_method);
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
                list.SetUpperDataName(DataName, FieldType.read_value);
                list.set_Inside_Read();
                Method m = list.GetGloabalMethod();
                if (m != null)
                {
                    listMethods.Add(m);
                }
            }
            return listMethods;
        }
    }
}
