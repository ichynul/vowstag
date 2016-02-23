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
using Tag.Vows.Interface;
using Tag.Vows.Tool;
namespace Tag.Vows.Tag
{
    abstract class StyleAbleTag : BaseTag, IStyleAble
    {
        protected string Style { get; private set; }

        protected StyleAbleTag(string mtext, string origin, int mdeep, TagConfig config, int no_)
            : base(mtext, origin, mdeep, config, no_)
        {

        }
        public void SetStyle(string style)
        {
            this.Style = style;
        }


        public bool HasStyle()
        {
            return !string.IsNullOrEmpty(this.Style);
        }

        public string GetStyle()
        {
            return this.Style;
        }
    }
}
