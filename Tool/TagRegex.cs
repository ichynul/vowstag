using System.Text.RegularExpressions;

namespace Tag.Vows
{
    class TagRegex
    {
        public TagRegex(string tagLeft, string tagRight)
        {
            #region 标签正则表达式
            /********tag tests*******/
            TagTest = string.Concat("(?s)", tagLeft, @"\s*[\w].*?/?", tagRight, "(?-s)");
            QueryBase = @"\w+(?:\?(?:(?:!?\(*)?[#$%]?\w+(?:=|!=|>|<|%|!%)(?:[\w\-/:]+|\-?\d+(?:\.\d+)?|""""|(\w+,?)*?|DateTime.Now(?:\.Add\w+\(-?\d+\))?|\w+\.\w+(?:[\+\-\*/]\d+(?:\.\d+)?)?)\)*(?:&|\||<[bh]r/?>)??)+?)?/?";
            ListTest = string.Concat(tagLeft, "list=", QueryBase, tagRight);
            ReadTest = string.Concat(tagLeft, "read=", QueryBase, tagRight);
            FormTest = string.Concat(tagLeft, "form=", QueryBase, tagRight);
            JsonTest = string.Concat(tagLeft, "json=", QueryBase, tagRight);
            LabelTest = string.Concat(tagLeft, @"label=\w+/?", tagRight);
            StaticTest = string.Concat(tagLeft, @"static=\w+/?", tagRight);
            FiledTest = string.Concat(tagLeft, @"\w+(?:\.\w+)*(?:\[""\w+""\])?/?", tagRight);
            MethodTest = string.Concat(tagLeft, @"\w+(?:\.\w+)*\([^\)]*\)/?", tagRight);
            PagerTest = string.Concat(tagLeft, @"pager(?:\?(?:\w+=.+(?:&|<br/?>)?)*)?/?", tagRight);
            CommandTest = string.Concat(tagLeft, @"cmd\?(?:\w+=[\w\.]+(?:&|<br/?>)?)+?/?", tagRight);

            /********wrong tests*******/
            RequestValue = @"request\.\w+";
            SessionValue = @"session\.\w+";
            CookieValue = @"cookie\.\w+";
            CallValue = @"call\.\w+";
            ItemValue = @"item\.\w+";
            ReadValue = @"read\.\w+";
            FormValue = @"form\.\w+";
            /********other tests*******/
            JsCssImageTest = @"(?s)<(?:script|link|img).*?(?:src|href)=['""](?<src>(?:\.\./)?[\w\-][\w\-\.]*/.*?\.(?:js|css|png|jpg|jpeg|gif|bmp))['""](?-s)";
            CssBgTest = @"(?s)background(?:\-image)?\s*:.*?url\s*\(['""]?(?<src>(?:\.\./)?[\w\-][\w\-\.]*/.*?\.(?:png|jpg|jpeg|gif|bmp))\s*['""]?\)(?-s)";
            ThisDirTest = @"^[\w\-][\w\-\.]*/.*$";
            OtherDirTest = @"^\.\./[\w\-][\w\-\.]*/.*$";
            ServerCodeTest = @"(?s)<%(?<code>.*?)%>(?-s)";
            ServerScriptTest = @"(?s)<script[^>]*?runat\s*=\s*['""]*\s*server\s*['""][^>]*?>(?<script>.*?)</script>(?-s)";
            /********pairs tests*******/
            TagPairEndTest = string.Concat("(?s)", tagLeft, @"\s*/?(?<name>(?:list|read|label|static|form|json|cmd|pager))\s*", tagRight);
            TagPairTest = string.Concat("(?s)", tagLeft, @"\s*(?<tag>list|read)\s*=\s*\w+(?:\?.*?[^/]\s*)?", tagRight,
                                                               "(?<style>.+?", tagLeft, @"\s*/?\1\s*", tagRight, ")(?-s)");
            EmptyPairTest = string.Concat("(?s)", tagLeft, @"\s*empty\s*", tagRight,
                                                               "(?<style>.+?)", tagLeft, @"\s*/?empty\s*", tagRight, "(?-s)");
            /********if tests*******/
            IfTest = string.Concat("(?s)", tagLeft, @"\s*if\s*\?(?<test>.+?)", tagRight, "(?<content>.+?)(?=", tagLeft, @"\s*/?(?:el(?:se)?if|else|endif).*?", tagRight, ")", "(?-s)");
            ElseIfTest = string.Concat("(?s)", tagLeft, @"\s*el(?:se)?if\s*\?(?<test>.+?)", tagRight, "(?<content>.+?)(?=", tagLeft, @"\s*/?(?:el(?:se)?if|else|endif).*?", tagRight, ")", "(?-s)");
            ElseTest = string.Concat("(?s)", tagLeft, @"\s*else\s*\", tagRight, "(?<content>.+?)(?=", tagLeft, @"\s*/?endif\s*", tagRight, ")", "(?-s)");
            IfPairTest = string.Concat("(?s)", tagLeft, @"\s*if\s*\?(.(?!", tagLeft, "))+?", tagRight, "(.(?!", tagLeft, @"\s*if", "))+?", tagLeft, @"\s*/?endif\s*", tagRight, "(?-s)");
            EmptyTest = string.Concat("(?s)", tagLeft, @"\s*empty\s*", tagRight);
            #endregion

            #region 标签内部字段提取正则表达式
            DataNameRegex = string.Concat(@"(?<=(?:list|read|label||form|json)=)\w+?(?=(\?|/?", tagRight, "))");
            ItemPathRegex = @"(?<=item=)\w+?(?=&|\||<[bh]r/?>|$)";
            BaseParamsRegex = string.Concat(@"(?<=\?).*?(?=/?", tagRight, ")");
            FiledObjRegex = string.Concat("(?<=", tagLeft, @")\w+(\.\w+)*(\[""\w+""\])?(?=/?", tagRight, ")");
            MethodObjRegex = string.Concat("(?<=", tagLeft, @")\w+(\.\w+)*\([^\)]*\)(?=/?", tagRight, ")");
            PagerRegex = string.Concat("(?<=", tagLeft, @")pager(\?(\w+=.+(&|<br/?>)?)*)?(?=/?", tagRight, ")");
            #endregion
        }

        #region 标签正则表达式
        /********tag tests*******/
        public string TagTest { private set; get; }
        private string QueryBase;
        public string ListTest { private set; get; }
        public string ReadTest { private set; get; }
        public string FormTest { private set; get; }
        public string JsonTest { private set; get; }
        public string LabelTest { private set; get; }
        public string StaticTest { private set; get; }
        public string FiledTest { private set; get; }
        public string MethodTest { private set; get; }
        public string PagerTest { private set; get; }
        public string CommandTest { private set; get; }
        public string IfTest { private set; get; }
        public string ElseIfTest { private set; get; }
        public string ElseTest { private set; get; }
        public string TagPairEndTest { private set; get; }
        public string EmptyPairTest { private set; get; }
        public string IfPairTest { private set; get; }
        public string EmptyTest { private set; get; }
        /********wrong tests*******/
        public string RequestValue { private set; get; }
        public string SessionValue { private set; get; }
        public string CookieValue { private set; get; }
        public string CallValue { private set; get; }
        public string ItemValue { private set; get; }
        public string ReadValue { private set; get; }
        public string FormValue { private set; get; }
        /********other tests*******/
        public string JsCssImageTest { private set; get; }
        public string CssBgTest { private set; get; }
        public string ThisDirTest { private set; get; }
        public string OtherDirTest { private set; get; }
        public string ServerCodeTest { private set; get; }
        public string ServerScriptTest { private set; get; }
        /********pairs tests*******/
        public string TagPairTest { private set; get; }
        #endregion

        #region 标签内部字段提取正则表达式
        protected string DataNameRegex { private set; get; }
        protected string ItemPathRegex { private set; get; }
        protected string BaseParamsRegex { private set; get; }
        protected string FiledObjRegex { private set; get; }
        protected string MethodObjRegex { private set; get; }
        protected string PagerRegex { private set; get; }

        #endregion

        #region 标签内部字段提取方法
        public string getDataName(string Text)
        {
            Match m = Regex.Match(Text, DataNameRegex, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }

        public string getItemPath(string baseParasm)
        {
            Match m = Regex.Match(baseParasm, ItemPathRegex, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }

        public string getBaseParams(string Text)
        {
            Match m = Regex.Match(Text, BaseParamsRegex, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return m.Value.EndsWith("&") ? m.Value.Remove(m.Value.Length - 1) : m.Value;
            }
            return string.Empty;
        }

        public string getFiledObj(string Text)
        {
            Match m = Regex.Match(Text, FiledObjRegex, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }

        public string getMethodObj(string Text)
        {
            Match m = Regex.Match(Text, MethodObjRegex, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }

        public string getPager(string Text)
        {
            Match m = Regex.Match(Text, PagerRegex, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return m.Value;
            }
            return string.Empty;
        }
        #endregion
    }
}