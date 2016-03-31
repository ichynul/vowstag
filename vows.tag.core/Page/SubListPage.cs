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
                else if (c is MethodTag)
                {
                    (c as MethodTag).SetDataName(DataName, MethodType.item_value_method);
                }
            }
        }
    }
}