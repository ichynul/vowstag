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
using System.Text;
using Tag.Vows.Bean;
using Tag.Vows.Enum;
using Tag.Vows.Interface;
using Tag.Vows.Tag;
using Tag.Vows.Tool;

namespace Tag.Vows.Page
{
    class ReadPage : BasePage, IUpperDataAble
    {
        public ReadPage(string mHtmlpPath, string mPageName, int mDeep, TagConfig config)
            : base(mHtmlpPath, mPageName, mDeep, config, false)
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

        public List<Method> GetListMethods(string DataName)
        {
            List<Method> listMethods = new List<Method>();
            StringBuilder sb = new StringBuilder();
            ListTag list = null;
            foreach (var x in this.TagList.Where(x => x is ListTag))
            {
                list = x as ListTag;
                list.SetUpperDataName(DataName, FieldType.read_value);
                list.set_Inside_Read();
                Method m = list.GetIGlobalMethod();
                if (m != null)
                {
                    listMethods.Add(m);
                }
            }
            return listMethods;
        }
    }
}
