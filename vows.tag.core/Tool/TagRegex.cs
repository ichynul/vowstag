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

namespace Tag.Vows.Tool
{
    sealed class TagRegex
    {
        #region 标签正则表达式
        /********tag tests*******/
        public Regex TagTest { private set; get; }
        private string QueryBase;
        public Regex ListTest { private set; get; }
        public Regex ReadTest { private set; get; }
        public Regex FormTest { private set; get; }
        public Regex JsonTest { private set; get; }
        public Regex LabelTest { private set; get; }
        public Regex StaticTest { private set; get; }
        public Regex FiledTest { private set; get; }
        public Regex MethodTest { private set; get; }
        public Regex PagerTest { private set; get; }
        public Regex CommandTest { private set; get; }
        public Regex IfTest { private set; get; }
        public Regex ElseIfTest { private set; get; }
        public Regex ElseTest { private set; get; }
        public Regex TagPairEndTest { private set; get; }
        public Regex ifTagKeyTest { private set; get; }
        public Regex EmptyPairTest { private set; get; }
        public Regex IfPairTest { private set; get; }
        public Regex EmptyTest { private set; get; }
        /********wrong tests*******/
        public Regex RequestValue { private set; get; }
        public Regex SessionValue { private set; get; }
        public Regex CookieValue { private set; get; }
        public Regex CookieValue_sub { private set; get; }
        public Regex CallValue { private set; get; }
        public Regex ItemValue { private set; get; }
        public Regex ReadValue { private set; get; }
        public Regex FormValue { private set; get; }
        public Regex NormalIndex { private set; get; }
        /********other tests*******/
        public Regex ServerCodeTest { private set; get; }
        public Regex ServerScriptTest { private set; get; }
        public Regex NotEmptyStringTest { private set; get; }
        /********pairs tests*******/
        public Regex TagPairTest { private set; get; }
        #endregion

        #region 标签内部字段提取正则表达式
        private Regex DataNameRegex { set; get; }
        private Regex ItemPathRegex { set; get; }
        private Regex BaseParamsRegex { set; get; }
        private Regex FiledObjRegex { set; get; }
        private Regex MethodObjRegex { set; get; }
        private Regex PagerRegex { set; get; }
        #endregion

        public TagRegex(string tagLeft, string tagRight)
        {

            QueryBase = string.Concat(@"\w+(?:\?(?:(?:!?\(*)?(?:|"".*?""|\[.*?\]|\w+|\w+\.\w+)+(?:>|<|!=|=|>=|<=|!%|%|!#|#)"
                            , @"(?:[\w\-\/:]+|\-?\d+(?:\.\d+)?|"".*?""|\[.*?\]|(\w+,?)*?|\w+(?:\.\w+)*\(.*?\)|"
                            , @"\w+(?:\.\w+)*(?:[\+\-\*\/]\d+(?:\.\d+)?)?)\)*(?:&|\||<[bh]r/?>)??)+?)?/?");
            #region 标签正则表达式
            /********tag tests*******/
            TagTest = new Regex(string.Concat(tagLeft, @"\s*\b.+?/?\s*", tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);

            ListTest = new Regex(string.Concat(tagLeft, @"list\d*=", QueryBase, tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            ReadTest = new Regex(string.Concat(tagLeft, "read=", QueryBase, tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            FormTest = new Regex(string.Concat(tagLeft, "form=", QueryBase, tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            JsonTest = new Regex(string.Concat(tagLeft, "json=", QueryBase, tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            LabelTest = new Regex(string.Concat(tagLeft, @"label=[\w\-]+/?", tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            StaticTest = new Regex(string.Concat(tagLeft, @"static=[\w\-]+/?", tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            FiledTest = new Regex(string.Concat(tagLeft, @"\w+(?:\.\w+)*?(?:\[""\w+""\])?/?", tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MethodTest = new Regex(string.Concat(tagLeft, @"\w+(?:\.\w+)*?\(.*?\)/?", tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PagerTest = new Regex(string.Concat(tagLeft, @"pager(?:\?(?:\w+=.+(?:&|<br/?>)?)*)?/?", tagRight)
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            CommandTest = new Regex(string.Concat(tagLeft, @"cmd\?(?:\w+=[\w\.,]+(?:&|<br/?>)?)+?/?", tagRight)
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            EmptyTest = new Regex(string.Concat(tagLeft, @"\s*empty\s*", tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            /********value tests*******/
            RequestValue = new Regex(@"(?:request|req|url)\.\w+\b", RegexOptions.IgnoreCase);
            SessionValue = new Regex(@"session\.\w+\b", RegexOptions.IgnoreCase);
            CookieValue = new Regex(@"cookie\.\w+\b", RegexOptions.IgnoreCase);
            CookieValue_sub = new Regex(@"cookie\.\w+\.\w+\b", RegexOptions.IgnoreCase);
            CallValue = new Regex(@"call\.\w+\b", RegexOptions.IgnoreCase);
            ItemValue = new Regex(@"item\.\w+\b", RegexOptions.IgnoreCase);
            ReadValue = new Regex(@"read\.\w+\b", RegexOptions.IgnoreCase);
            FormValue = new Regex(@"form\.\w+\b", RegexOptions.IgnoreCase);
            NormalIndex = new Regex(@"^.*?(\[[\w""]+\]).*?$", RegexOptions.IgnoreCase);
            ServerCodeTest = new Regex(@"<%(?<code>.*?)%>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            ServerScriptTest = new Regex(@"<script[^>]*?runat\s*=\s*['""]*\s*server\s*['""][^>]*?>(?<script>.*?)</script>"
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            NotEmptyStringTest = new Regex(string.Concat(@"(?<=(?:!=|!%|=|%)\s*)"".+?""(?=\s*(?:&|\||\)|", tagRight, "))"));
            /********pairs tests*******/
            TagPairEndTest = new Regex(string.Concat(tagLeft, @"\s*(?<endstr>/?)\s*(?<name>list|read|label|static|form|json|cmd|pager)\s*", tagRight)
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            ifTagKeyTest = new Regex(string.Concat(tagLeft, @"\s*(?:else|endif)\s*", tagRight), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            TagPairTest = new Regex(string.Concat(tagLeft, @"\s*(?<tag>list|read)\s*=\s*\w+(?:\?.*?[^/]\s*)?", tagRight,
                                                               "(?<style>.+?", tagLeft, @"\s*/?\1\s*", tagRight, ")(?-s)")
                                                               , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            EmptyPairTest = new Regex(string.Concat(tagLeft, @"\s*empty\s*", tagRight, "(?<style>.+?)", tagLeft, @"\s*/?empty\s*", tagRight, "(?-s)")
                                                               , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            /********if tests*******/
            IfTest = new Regex(string.Concat(tagLeft, @"\s*if\s*\?(?<test>.+?)", tagRight, "(?<content>.+?)(?=", tagLeft,
                                @"\s*/?(?:el(?:se)?if|else|endif).*?", tagRight, ")"), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            ElseIfTest = new Regex(string.Concat(tagLeft, @"\s*el(?:se)?if\s*\?(?<test>.+?)", tagRight, "(?<content>.+?)(?=", tagLeft, @"\s*/?(?:el(?:se)?if|else|endif).*?", tagRight, ")")
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            ElseTest = new Regex(string.Concat(tagLeft, @"\s*else\s*\", tagRight, "(?<content>.+?)(?=", tagLeft, @"\s*/?\s*endif\s*", tagRight, ")")
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            IfPairTest = new Regex(string.Concat(tagLeft, @"\s*if\s*\?(.(?!", tagLeft, "))+?", tagRight, "(.(?!", tagLeft, @"\s*if", "))+?", tagLeft, @"\s*/?\s*endif\s*", tagRight)
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);
            #endregion

            #region 标签内部字段提取正则表达式
            DataNameRegex = new Regex(string.Concat(@"(?<=(?:list|read|label||form|json)=)[\w\-]+?(?=(\?|/?", tagRight, "))"), RegexOptions.IgnoreCase);
            ItemPathRegex = new Regex(@"(?<=item=)[\w\-]+?(?=&|\||<[bh]r/?>|$)", RegexOptions.IgnoreCase);
            BaseParamsRegex = new Regex(string.Concat(@"(?<=\?).*?(?=/?", tagRight, ")"), RegexOptions.IgnoreCase);
            FiledObjRegex = new Regex(string.Concat("(?<=", tagLeft, @")\w+(?:\.\w+)*?(\[""\w+""\])?(?=/?", tagRight, ")"), RegexOptions.IgnoreCase);
            MethodObjRegex = new Regex(string.Concat("(?<=", tagLeft, @")\w+(?:\.\w+)*?\(.*?\)(?=/?", tagRight, ")"), RegexOptions.IgnoreCase);
            PagerRegex = new Regex(string.Concat("(?<=", tagLeft, @")pager(\?(?:\w+=.+(&|<br/?>)?)*)?(?=/?", tagRight, ")"), RegexOptions.IgnoreCase);
            #endregion
        }

        #region 标签内部字段提取方法
        public string getDataName(string Text)
        {
            Match m = DataNameRegex.Match(Text);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }

        public string getItemPath(string baseParasm)
        {
            Match m = ItemPathRegex.Match(baseParasm);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }

        public string getBaseParams(string Text)
        {
            Match m = BaseParamsRegex.Match(Text);
            if (m.Success)
            {
                return m.Value.EndsWith("&") ? m.Value.Remove(m.Value.Length - 1) : m.Value;
            }
            return string.Empty;
        }

        public string getFiledObj(string Text)
        {
            Match m = FiledObjRegex.Match(Text);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }

        public string getMethodObj(string Text)
        {
            Match m = MethodObjRegex.Match(Text);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }

        public string getPager(string Text)
        {
            Match m = PagerRegex.Match(Text);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }
        #endregion
    }
}
