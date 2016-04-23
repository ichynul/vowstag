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
namespace Tag.Vows.Bean
{
    /// <summary>
    /// ShowTag bean
    /// </summary>
    public class ShowTag
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="n">no_</param>
        /// <param name="t">text</param>
        /// <param name="o">origin</param>
        /// <param name="i">in_pairs</param>
        public ShowTag(int n, string t, string o, bool i)
        {
            no_ = n;
            text = t;
            origin = o;
            in_pairs = i;
        }

        /// <summary>
        /// no_
        /// </summary>
        public int no_;
        /// <summary>
        /// text
        /// </summary>
        public string text;
        /// <summary>
        /// origin
        /// </summary>
        public string origin;
        /// <summary>
        /// in_pairs
        /// </summary>
        public bool in_pairs;
        /// <summary>
        /// style
        /// </summary>
        public string style;
    }
}
