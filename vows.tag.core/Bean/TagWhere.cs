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
using System.Text.RegularExpressions;

namespace Tag.Vows.Bean
{
    class TagWhere
    {
        private TagWhere() { }
        public TagWhere(string mFiledName, string mCompare, string mLogicSymb, string mVarName)
        {
            this.FiledName = Regex.Replace(mFiledName, @"\W", "").ToLower();
            this.Compare = mCompare;
            this.LogicSymb = mLogicSymb;

            Match m = Regex.Match(mVarName, @"(?<varname>.+?)(?<varright>\)+?)(?=$)");
            if (m.Success)
            {
                this.VarRight = m.Groups["varright"].Value;
                this.VarName = m.Groups["varname"].Value;
            }
            else
            {
                this.VarName = mVarName;
            }
            m = Regex.Match(mFiledName, @"(?<=^)[!\(]+(?=\w)");
            if (m.Success)
            {
                this.FieldLeft = m.Value;
            }

        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string FiledName { get; set; }
        /// <summary>
        /// 比较符
        /// </summary>
        public string Compare { get; set; }
        /// <summary>
        /// 逻辑连接符
        /// </summary>
        public string LogicSymb { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string VarName { get; set; }
        /// <summary>
        /// 属性左边
        /// </summary>
        public string FieldLeft { get; set; }
        /// <summary>
        /// 属性值右边
        /// </summary>
        public string VarRight { get; set; }
    }
}
