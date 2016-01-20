/*
* The MIT License (MIT)

*Copyright (c) 2015 ichynul

*Permission is hereby granted, free of charge, to any person obtaining a copy
*of this software and associated documentation files (the "Software"), to deal
*in the Software without restriction, including without limitation the rights
*to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*copies of the Software, and to permit persons to whom the Software is
*furnished to do so, subject to the following conditions:

*The above copyright notice and this permission notice shall be included in all
*copies or substantial portions of the Software.

*THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
*LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
*OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
*SOFTWARE.
*/


using System.Text.RegularExpressions;

namespace Tag.Vows.Bean
{
    class mWhere
    {
        private mWhere() { }
        public mWhere(string mFiledName, string mCompare, string mLogicSymb, string mVarName)
        {
            this.NotDataField = Regex.IsMatch(mFiledName, "^[#$%]");
            this.FiledName = Regex.Replace(mFiledName, @"\W", "").ToLower();
            this.Compare = mCompare;
            this.LogicSymb = mLogicSymb;
            this.VarName = Regex.Replace(mVarName, @"[^\w\-:\.,]", "");
            Match m = Regex.Match(mFiledName, @"(?<=^)[!\(]+(?=\w)");
            if (m.Success)
            {
                this.FieldLeft = m.Value;
            }
            m = Regex.Match(mVarName, @"(?<=\w)\)+(?=$)");
            if (m.Success)
            {
                this.VarRight = m.Value;
            }
        }

        public string FiledName { get; set; }
        public string Compare { get; set; }
        public string LogicSymb { get; set; }
        public string VarName { get; set; }
        public string FieldLeft { get; set; }
        public string VarRight { get; set; }
        public bool NotDataField { get; set; }
    }
}
