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
using System.Text;
using System.Linq;

namespace Tag.Vows.Bean
{
    /// <summary>
    ///Method 的摘要说明
    /// </summary>
    class Method
    {
        public Method()
        {
            Body = new StringBuilder();
        }

        public string Name { get; set; }
        public string ReturnType { get; set; }
        public StringBuilder Body { get; set; }
        public bool InPageLoad { get; set; }
        public string ParsmStr { get; set; }
        public string UseParams { get; set; }

        public bool WillTestBeforLoad { get; set; }
        protected List<string> TestsBeforLoad { get; set; }
        public static readonly string Space = "    ";

        private static Dictionary<int, string> Spaces = new Dictionary<int, string>();

        public void SetTestBeforLoad(HashSet<string> tests)
        {
            if (tests != null && tests.Count > 0)
            {
                this.TestsBeforLoad = new List<string>();
                var list = tests.AsEnumerable();
                for (int i = tests.Count - 1; i > -1; i -= 1)
                {
                    this.TestsBeforLoad.Add(list.ElementAt(i));
                }
            }
        }

        public string GetTestBeforLoadStr()
        {
            if (this.TestsBeforLoad != null)
            {
                if (this.TestsBeforLoad.Count > 1)
                {
                    string sp = string.Concat(")\r\n", Method.getSpaces(5), "&& (");
                    return string.Concat("(", string.Join(sp, this.TestsBeforLoad), ")");
                }
                else if (this.TestsBeforLoad.Count == 1)
                {
                    return this.TestsBeforLoad.First();
                }
            }
            return string.Empty;
        }
        public string ToFullMethodRect()
        {
            return string.Concat(Space, "public ", ReturnType == null ? "void" : ReturnType, " ", Name, "(", ParsmStr, ")\r\n"
                , Space, "{\r\n", Body, Space,
                "}\r\n\r\n");
        }

        public static string getSpaces(int times)
        {
            if (Spaces.Keys.Contains(times))
            {
                return Spaces[times];
            }
            string str = string.Empty;
            if (times > 0)
            {
                for (int i = 0; i < times; i += 1)
                {
                    str += Space;
                }
            }
            Spaces.Add(times, str);
            return str;
        }
    }
}