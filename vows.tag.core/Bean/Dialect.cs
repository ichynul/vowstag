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
using Tag.Vows.Interface;
namespace Tag.Vows.Bean
{
    /// <summary>
    /// 方言
    /// </summary>
    public class Dialect : IDialectAble
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="otherNames"></param>
        public Dialect(string name, HashSet<string> otherNames)
        {
            Name = name;
            OtherNames = otherNames;
        }
        /// <summary>
        /// 本名
        /// </summary>
        public string Name;
        /// <summary>
        /// 其他方言集合
        /// </summary>
        protected HashSet<string> OtherNames;

        /// <summary>
        /// 匹配方言
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool MatchName(string name)
        {
            if (OtherNames == null)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(this.Name))
            {
                if (this.Name.ToLower() == name.ToLower())
                {
                    return true;
                }
            }
            name = name.ToLower();
            foreach (var x in OtherNames)
            {
                if (x.ToLower() == name)
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 表及字段方言
    /// </summary>
    public class TableDialect : Dialect
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="otherNames"></param>
        public TableDialect(string name, HashSet<string> otherNames)
            : base(name, otherNames)
        {
        }
        /// <summary>
        /// 字段方言
        /// </summary>
        public List<Dialect> Fields = new List<Dialect>();
    }
}
