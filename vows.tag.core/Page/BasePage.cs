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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tag.Vows.Bean;
using Tag.Vows.Enum;
using Tag.Vows.Interface;
using Tag.Vows.Tag;
using Tag.Vows.Tool;

namespace Tag.Vows.Page
{
    /// <summary>
    ///At 2015/07/15
    ///By ichynul@outlook.com
    /// </summary>
    class BasePage : IHtmlAble
    {
        //
        protected TagConfig Config;
        private string Extends;
        private string SubpageExtends = "SubControl";
        private bool? CallBack = null;
        private bool ValidateRequest = true;
        private bool EnableViewState = false;
        //
        private string TagCallBack = "";
        public string Html { get; protected set; }
        protected string HtmlpPath;
        public string PageName { get; protected set; }
        protected string Msg = "";
        protected string AspxCode { get; private set; }
        protected string AspxCsCode { get; private set; }
        protected string ext = ".html";
        protected MatchCollection Matches, _Matches;
        protected Match TheMatch;
        protected StringBuilder MethodLines;
        protected StringBuilder MethodRects;
        protected StringBuilder CallMethods;
        protected string GloabalFileds = "";
        public List<BaseTag> TagList = new List<BaseTag>();
        public List<Method> MethodsInPage = new List<Method>();
        protected int Deep;
        protected string[] colors = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
        protected BasePage() { }

        public BasePage(string mPageName, TagConfig config)
            : this(string.Empty, mPageName, 1, config)
        {
        }

        /// <summary>
        /// 页面入口方法,以文件路径
        /// </summary>
        /// <param name="mHtmlpPath">somepage.html文件路径</param>
        /// <param name="mPageName">页面名称,如: somepage </param>
        /// <param name="mDeep">嵌套深度</param>
        /// <param name="config">配置信息</param>
        public BasePage(string mHtmlpPath, string mPageName, int mDeep, TagConfig config)
        {
            config.Init();
            this.HtmlpPath = string.IsNullOrEmpty(mHtmlpPath) ? config.PagePath : mHtmlpPath;
            this.Config = config;
            this.Extends = config.DefaultBase;
            this.Deep = mDeep + 1;

            this.PageName = mPageName;
            if (Deep > config.MAXD_EEP)
            {
                this.Msg += string.Format("{0}-镶套层数达到{1}层，为防止循环套用已停止解析。<br />", this.PageName, config.MAXD_EEP);
                return;
            }
            GetTegs(false);
        }

        /// <summary>
        /// 页面入口方法,以html文本
        /// </summary>
        /// <param name="style">html文本</param>
        /// <param name="mDeep">嵌套深度</param>
        /// <param name="config">配置信息</param>
        /// <param name="fakeName">虚拟的页面名称</param>
        public BasePage(string style, int mDeep, TagConfig config, string fakeName)
        {
            this.Config = config;
            this.Extends = config.DefaultBase;
            this.Deep = mDeep + 1;
            this.PageName = fakeName;
            this.Html = style;
            if (Deep > config.MAXD_EEP)
            {
                this.Msg += string.Format("{0}-镶套层数达到{1}层，为防止循环套用已停止解析。<br />", this.PageName, config.MAXD_EEP);
                return;
            }
            GetTegs(true);
        }

        public string GetMsg()
        {
            return this.Msg;
        }

        /// <summary>
        /// 解析所有标签
        /// </summary>
        /// <param name="style">是否是以文本初始化</param>
        protected void GetTegs(bool style)
        {
            if (!style)//以路径初始化，读取指定文件内容
            {
                if (!Directory.Exists(HtmlpPath))
                {
                    Msg += string.Format("不存在该路径：{0}<br />", HtmlpPath);
                    Html = string.Format("<!--不存在该路径：{0} -->", HtmlpPath);
                    return;
                }
                string templatePath = HtmlpPath + PageName + ".html";
                if (!File.Exists(templatePath))
                {
                    templatePath = HtmlpPath + PageName + ".htm";
                    ext = ".htm";
                }
                if (!File.Exists(templatePath))
                {
                    templatePath = HtmlpPath + PageName + ".txt";
                    ext = ".txt";
                }
                if (!File.Exists(templatePath))
                {
                    Msg += string.Format("不存在该文件：{0}，请检查文件名或文件后缀（支持*.html、*.htm、*.txt）<br />", templatePath);
                    Html = string.Format("<!--不存在该文件：{0} -->", templatePath);
                    return;
                }
                try
                {
                    Html = File.ReadAllText(templatePath);
                }
                catch (Exception e)
                {
                    Msg += string.Format("无法打开文件:{0}。  {1}", templatePath, e.Message);
                    return;
                }
            }
            if (string.IsNullOrEmpty(Html))
            {
                Msg += string.Format("无内容，已终止。");
                return;
            }
            Html = Config.GetingHtml(Html);
            ReplaceNotes();
            if (!Config.allowServerScript)
            {
                DissAbleServerScript();
            }
            ReplaceSpacesAndMatchAll();
            if (!this.Config.convert)
            {
                RePathJsCssImg();
            }
            ReplaceEnd(false);
            FindListOrReadPairs(Html, 1);
            ReplaceTagAndLoadList();
            if (!this.Config.convert)
            {
                FindIfPairs(Html);
            }
            InitTestToLoadTag();
            ReplaceEnd(true);
            //
            DoForForm();
            if (!(this is SubListPage))
            {
                DoForPager();
            }
            DoForRead();
        }

        /// <summary>
        /// 服务端代码安全检测 禁用 &lt;% ...%&gt;和%&lt;cript runat="server"&gt;....&lt;/script&gt;
        /// </summary>
        private void DissAbleServerScript()
        {
            string value = "";
            string g = "";
            Matches = this.Config.tagregex.ServerCodeTest.Matches(Html);
            for (int i = 0; i < Matches.Count; i += 1)
            {
                value = Matches[i].Value;
                g = Matches[i].Groups["code"].Value;
                Html = Html.Replace(value, value.Replace(g, string.Concat("\r\n    /*当前设置不允许服务端代码\r\n",
                    g, "\r\n    当前设置不允许服务端代码*/\r\n    ")));
            }
            Matches = this.Config.tagregex.ServerScriptTest.Matches(Html);
            for (int i = 0; i < Matches.Count; i += 1)
            {
                value = Matches[i].Value;
                g = Matches[i].Groups["script"].Value;
                Html = Html.Replace(value, value.Replace(g, string.Concat("\r\n    /*当前设置不允许服务端代码\r\n",
                    g, "\r\n    当前设置不允许服务端代码*/\r\n    ")));
            }
        }

        private string FindStringAndReplace(string tagText)
        {
            _Matches = this.Config.tagregex.NotEmptyStringTest.Matches(tagText);
            string str;
            for (int i = 0; i < _Matches.Count; i += 1)
            {
                str = _Matches[i].Value;
                tagText = tagText.Replace(str, Regex.Replace(str, @"\s", "x_spcce_x", RegexOptions.Singleline));
            }
            return tagText;
        }

        private string RecoverStringAndReplace(string tagText)
        {
            tagText = Regex.Replace(tagText, @"x_spcce_x", " ", RegexOptions.Singleline);
            return tagText;
        }

        /// <summary>
        /// 替换注释 <!--# notes -->
        /// </summary>
        protected void ReplaceNotes()
        {
            this.Html = Regex.Replace(this.Html, @"<!--\s*#.*?-->", string.Empty, RegexOptions.Singleline);
        }

        /// <summary>
        /// 将可能的标签格式化及去除空格，以作进一步匹配
        /// </summary>
        private void ReplaceSpacesAndMatchAll()
        {
            Matches = this.Config.tagregex.TagTest.Matches(Html);
            string tag = "", origin = "";
            for (int i = 0; i < Matches.Count; i += 1)
            {
                origin = Matches[i].Value;
                tag = FindStringAndReplace(origin);
                tag = Regex.Replace(tag, @"\s", string.Empty, RegexOptions.IgnoreCase);
                tag = RecoverStringAndReplace(tag);
                tag = Regex.Replace(tag, @"<br/>", "&", RegexOptions.IgnoreCase);
                tag = Regex.Replace(tag, @"<hr/>", "|", RegexOptions.IgnoreCase);
                //
                MatchTag(tag, origin, this.Config.tagregex.StaticTest, TagType._tag_static);
                MatchTag(tag, origin, this.Config.tagregex.FiledTest, TagType._tag_filed);
                MatchTag(tag, origin, this.Config.tagregex.MethodTest, TagType._tag_method);
                if (this is StaticPage)
                {
                    continue;
                }
                MatchTag(tag, origin, this.Config.tagregex.ListTest, TagType._tag_list);
                MatchTag(tag, origin, this.Config.tagregex.ReadTest, TagType._tag_read);
                MatchTag(tag, origin, this.Config.tagregex.LabelTest, TagType._tag_label);
                MatchTag(tag, origin, this.Config.tagregex.CommandTest, TagType._tag_command);
                if (this is LabelPage)
                {
                    continue;
                }
                if (!(this is SubListPage))
                {
                    MatchTag(tag, origin, this.Config.tagregex.PagerTest, TagType._tag_pager);
                }
                MatchTag(tag, origin, this.Config.tagregex.FormTest, TagType._tag_form);
                MatchTag(tag, origin, this.Config.tagregex.JsonTest, TagType._tag_json);
            }
        }

        /// <summary>
        /// 测试标签
        /// </summary>
        /// <param name="text">文本（经过格式化）</param>
        /// <param name="origin">原始文本</param>
        /// <param name="regex">正则表达式</param>
        /// <param name="type">目标标签类型</param>
        private void MatchTag(string text, string origin, Regex regex, TagType type)
        {
            if (this.Config.tagregex.TagPairEndTest.IsMatch(text) || this.Config.tagregex.EmptyTest.IsMatch(text)
                                || this.Config.tagregex.ifTagKeyTest.IsMatch(text))
            {
                return;
            }
            TheMatch = regex.Match(text);
            if (TheMatch.Success)
            {
                this.Html = this.Html.Replace(origin, text);
                if (type == TagType._tag_list)
                {
                    text = FindFirstListTagStr(text, this.TagList.Count());
                }
                if (type == TagType._tag_command)
                {
                    CMDTag tag = new CMDTag(text, origin, Deep, this.Config, this.TagList.Count);
                    GetCMD(tag);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_list)
                {
                    if (this is SubListPage)
                    {
                        ListTag tag = new ListTag(text, origin, Deep, (this as SubListPage).PageName, this.Config, this.TagList.Count);
                        this.TagList.Add(tag);
                    }
                    else
                    {
                        ListTag tag = new ListTag(text, origin, Deep, this is ItemPage ? (this as ItemPage).PageName : "", this.Config, this.TagList.Count);
                        this.TagList.Add(tag);
                    }
                }
                else if (type == TagType._tag_read)
                {
                    ReadTag tag = new ReadTag(text, origin, Deep, this.Config, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_label)
                {
                    LabelTag tag = new LabelTag(text, origin, Deep, this is LabelPage ? (this as LabelPage).PageName : "", this.Config, this.TagList.Count);
                    tag.LazyLoad();
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_static)
                {
                    StaticTag tag = new StaticTag(text, origin, Deep, this is StaticPage ? (this as StaticPage).PageName : "", this.Config, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_filed)
                {
                    FieldTag tag = new FieldTag(text, origin, Deep, this.Config, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_method)
                {
                    MethodTag tag = new MethodTag(text, origin, Deep, this.Config, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_pager)
                {
                    PagerTag tag = new PagerTag(text, origin, Deep, this.Config, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_form)
                {
                    FormTag tag = new FormTag(text, origin, Deep, this.Config, this.TagList.Count);
                    this.TagList.Add(tag);
                    this.TagCallBack = "form";
                }
                else if (type == TagType._tag_json)
                {
                    JsonTag tag = new JsonTag(text, origin, Deep, this.Config, this.TagList.Count);
                    this.TagList.Add(tag);
                    this.TagCallBack = "json";
                }
            }
        }

        /// <summary>
        /// 查找重复的listTag,在结尾加上序号以区分，避免替换的时候Repeater ID重复
        /// </summary>
        /// <param name="text">tagtext</param>
        /// <param name="deep">深度</param>
        /// <returns></returns>
        protected string FindFirstListTagStr(string text, int deep)
        {
            if (Html.LastIndexOf(text) != Html.IndexOf(text))
            {
                string p1, p2;
                int index = Html.IndexOf(text);
                p1 = Html.Substring(0, index);
                p2 = Html.Substring(index + text.Length);
                text = Regex.Replace(text, @"(?is)list\s*=\s*(?-i-s)", "list" + deep + "=");
                Html = string.Concat(p1, text, p2);
            }
            return text;
        }

        /// <summary>
        /// 替换html中的image、js、css background-img等资源的路径
        /// </summary>
        protected void RePathJsCssImg()
        {
            Matches = this.Config.tagregex.JsCssImageTest.Matches(Html);
            ReplaceLinks();
            Matches = this.Config.tagregex.CssBgTest.Matches(Html);
            ReplaceLinks();
        }

        private void ReplaceLinks()
        {
            GroupCollection gc = null;
            string src = "";
            List<string> r = new List<string>();
            for (int i = 0; i < Matches.Count; i += 1)
            {
                gc = Matches[i].Groups;
                src = gc["src"].Value;
                if (r.Contains(src))
                {
                    continue;
                }
                if (this.Config.tagregex.ThisDirTest.IsMatch(src))
                {
                    Html = Html.Replace(src, string.Format("{0}{1}/{2}", Config.input.Replace("~", ""), this is LabelPage ? "label" :
                        this is StaticPage ? "static" : this is SubListPage ? "item" : "page", src));
                }
                else if (this.Config.tagregex.OtherDirTest.IsMatch(src))
                {
                    Html = Html.Replace(src, string.Format("{0}{1}", Config.input.Replace("~", ""), src.Replace("../", "")));
                }
                r.Add(src);
            }
        }

        /// <summary>
        /// 获取cmd标签的内容
        /// </summary>
        /// <param name="cmd"></param>
        private void GetCMD(CMDTag cmd)
        {
            string value = cmd.QueryString("base");
            if (!string.IsNullOrEmpty(value))
            {
                if (this is LabelPage)
                {
                    this.SubpageExtends = value;
                }
                else if (this is BasePage)
                {
                    this.Extends = value;
                }
            }
            //
            value = cmd.QueryString("callback");
            if (!string.IsNullOrEmpty(value))
            {
                this.CallBack = value.ToLower() == "true";
            }
            //
            value = cmd.QueryString("validaterequest");
            this.ValidateRequest = value != null
                && value.ToLower() == "true";
            value = cmd.QueryString("enableviewstate");
            this.EnableViewState = string.IsNullOrEmpty(value)
                || value.ToLower() == "false";
        }

        /// <summary>
        /// for Pager
        /// </summary>
        private void DoForPager()
        {
            var theTag = TagList.FirstOrDefault(a => a is PagerTag);
            if (theTag != null)
            {
                foreach (var c in TagList.Where(x => x is ListTag))
                {
                    (c as ListTag).setPager((theTag as PagerTag));
                }
            }
        }

        /// <summary>
        /// for Read
        /// </summary>
        private void DoForRead()
        {
            var theTag = TagList.FirstOrDefault(a => a is ReadTag && !(a as ReadTag).HasStyle());
            if (theTag != null)
            {
                string dataName = (theTag as ReadTag).DataName;
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
                    else if (c is ListTag)
                    {
                        (c as ListTag).SetUpperDataName(dataName, FieldType.read_value);
                    }
                    else if (c is ReadTag)
                    {
                        (c as ReadTag).SetUpperDataName(dataName, FieldType.read_value);
                    }
                }
            }
        }

        /// <summary>
        /// for Form
        /// </summary>
        private void DoForForm()
        {
            string dataName = "";
            var theTag = TagList.FirstOrDefault(a => a is FormTag);
            if (theTag != null)
            {
                FormTag form = theTag as FormTag;
                dataName = form.DataName;
                FieldTag field = null;
                List<FieldTag> Fields = new List<FieldTag>();
                foreach (var c in TagList.Where(a => a is FieldTag))
                {
                    if (c is FieldTag)
                    {
                        field = c as FieldTag;
                        if (field.Type == FieldType.form_value)
                        {
                            field.SetDataName(dataName, FieldType.form_value);
                            form.AddField(field);
                        }
                    }
                }

                theTag = TagList.FirstOrDefault(a => a is MethodTag && (a as MethodTag).Type == MethodType.form_method);
                if (theTag != null)
                {
                    MethodTag method = theTag as MethodTag;
                    form.SetMethod(method);
                }
            }
        }

        /// <summary>
        /// 清除标签的结尾符号
        /// </summary>
        /// <param name="includelistAndRead"></param>
        private void ReplaceEnd(bool includelistAndRead)
        {
            Matches = this.Config.tagregex.TagPairEndTest.Matches(Html);
            string name = "";
            foreach (Match m in Matches)
            {
                name = m.Groups["name"].Value.ToLower();
                if (Config.convert)
                {
                    Html = Html.Replace(m.Value, Config.convert_pairs[0] + m.Groups["endstr"].Value + name + Config.convert_pairs[1]);
                    continue;
                }
                if (!includelistAndRead && (name == "list" || name == "read"))
                {
                    continue;
                }
                Html = Html.Replace(m.Value, string.Empty);
            }
        }

        /// <summary>
        /// 查找可能嵌套的list、read
        /// </summary>
        /// <param name="text"></param>
        /// <param name="deep"></param>
        private void FindListOrReadPairs(string text, int deep)
        {
            if (deep > 10)
            {
                return;
            }
            Matches = this.Config.tagregex.TagPairTest.Matches(text);
            string style = "";
            string tag = "";
            IStyleAble st = null;
            foreach (Match m in Matches)
            {
                style = m.Groups["style"].Value;
                tag = m.Groups["tag"].Value;
                if (tag.ToLower() == "list" && this.Config.tagregex.ListTest.IsMatch(style))
                {
                    FindListOrReadPairs(style, deep + 1);
                    continue;
                }
                if (tag.ToLower() == "read" && this.Config.tagregex.ReadTest.IsMatch(style))
                {
                    FindListOrReadPairs(style, deep + 1);
                    continue;
                }
                st = this.TagList.FirstOrDefault(x => x is IStyleAble && x.In_Pairs && m.Value.Contains(x.Text)) as IStyleAble;
                if (st != null & !string.IsNullOrEmpty(style))
                {
                    st.SetStyle(style);
                    if (st is ReadTag)
                    {
                        (st as ReadTag).LoadSubPage();
                    }
                    Html = Html.Replace(style, string.Empty);
                }
            }
        }

        /// <summary>
        /// 查找ifgroup
        /// </summary>
        /// <param name="text">文本</param>
        protected void FindIfPairs(string text)
        {
            Matches = this.Config.tagregex.IfPairTest.Matches(text);
            foreach (Match m in Matches)
            {
                IfGroupTag ifGroup = new IfGroupTag(m.Value, this.Deep, this.Config, this.TagList.Count);
                this.TagList.Add(ifGroup);
                this.Html = this.Html.Replace(m.Value, ifGroup.GetPlaceholderName());
            }
            if (Matches.Count > 0)
            {
                FindIfPairs(this.Html);
            }
        }

        /// <summary>
        /// 初始化TestToLoadTag
        /// </summary>
        protected void InitTestToLoadTag()
        {
            var tesToLoad = this.TagList.Where(x => x is ITesBeforLoading);
            var ifGroup = this.TagList.Where(x => x is ITestGroup);
            foreach (var x in tesToLoad)
            {
                foreach (var y in ifGroup)
                {
                    (y as ITestGroup).CheckTestToLoadTag(x as ITesBeforLoading);
                }
            }
        }

        /// <summary>
        /// 转换所有标签Text 并延迟加载ListTag
        /// </summary>
        protected void ReplaceTagAndLoadList()
        {
            foreach (var x in this.TagList)
            {
                if (x is ListTag)
                {
                    (x as ListTag).LazyLoad();
                }
                if (!this.Config.convert)
                {
                    this.Html = x.ReplaceTagText(this.Html);
                }
            }
        }

        /// <summary>
        /// 恢复ifgroup
        /// </summary>
        protected void RecoverIfGroupTags()
        {
            foreach (var c in this.TagList.Where(x => x is IfGroupTag).OrderByDescending(x => x.NO_))
            {
                this.Html = c.RecoverTagText(this.Html);
                this.Msg += c.GetMsg();
            }
        }

        /// <summary>
        /// /// 恢复其他
        /// </summary>
        protected void RecoverOtherTags()
        {
            ITableUseable ck = null;
            foreach (var c in this.TagList.OrderByDescending(x => x.NO_))
            {
                if (c is ITableUseable)
                {
                    ck = c as ITableUseable;
                    if (!ck.CheckDataUseable())
                    {
                        Html = Html.Replace(c.Text, string.Format("<!--{0}-->", ck.TabledisAbledMsg())) + "\r\n";
                        this.Msg += string.Concat(ck.TabledisAbledMsg(), "，在页面：", this.PageName, this.ext, "<br />");
                    }
                }
                Html = c.RecoverTagText(this.Html);
                this.Msg += c.GetMsg();
            }
        }

        /// <summary>
        /// 获取页面aspx 
        /// </summary>
        /// <returns></returns>
        public virtual string GetAspxCode()
        {
            this.RecoverIfGroupTags();
            this.RecoverOtherTags();
            if (!(this is IUC) && (this.TagCallBack == "json" || this.TagCallBack == "form" ||
                (this.CallBack == null && Config.creatScriptForAllPages) || this.CallBack == true))
            {
                TheMatch = Regex.Match(Html, "</body>", RegexOptions.IgnoreCase);
                if (TheMatch.Success)
                {
                    TheMatch = Regex.Match(Html, "<form[^>]*>", RegexOptions.IgnoreCase);
                    if (TheMatch.Success)
                    {
                        if (!Regex.IsMatch(TheMatch.Value, @"runat=['""]server['""]", RegexOptions.IgnoreCase))
                        {
                            Html = Html.Replace(TheMatch.Value, Regex.Replace(TheMatch.Value, ">$",
                                " runat=\"server\">", RegexOptions.IgnoreCase));
                        }
                    }
                    else
                    {
                        TheMatch = Regex.Match(Html, "</body>", RegexOptions.IgnoreCase);
                        if (TheMatch.Success)
                        {
                            Html = Html.Replace(TheMatch.Value, string.Concat("\r\n<form id=\"form1\" runat=\"server\"></form>\r\n", TheMatch.Value));
                        }
                    }
                    TheMatch = Regex.Match(Html, "</form>", RegexOptions.IgnoreCase);
                    if (TheMatch.Success)
                    {
                        Html = Html.Replace("</form>", string.Concat(JsMaker.GetCallBackJs().ToString(), "</form>"));
                    }
                    else
                    {
                        Html = string.Concat(Html, "<!-无法定位</form>-->");
                    }
                }
            }
            else if (this is IUC && this.CallBack == true)
            {
                Html = Html + "<!--仅顶级页面支持 callback 参数 -->>"; ;
            }

            return Html;
        }

        protected void FindAllMethodsOrFileds()
        {
            Method method = null;
            MethodLines = new StringBuilder();
            MethodRects = new StringBuilder();
            CallMethods = new StringBuilder();
            IIGlobalMethod loadMethod = null;
            ICallBackAble call = null;
            string field = "";
            foreach (var c in this.TagList.OrderByDescending(x => x.Lev))
            {
                if (c is ICallBackAble)
                {
                    call = c as ICallBackAble;
                    method = call.GetCallMethod();
                    GetMethodsLines(method);
                    CallMethods.AppendFormat("{0}callback = {1}();\r\n", Method.getSpaces(2), method.Name);
                    CallMethods.AppendFormat("{0}if (callback != null)\r\n", Method.getSpaces(2));
                    CallMethods.Append(Method.getSpaces(2) + "{\r\n");
                    CallMethods.AppendFormat("{0}return callback;\r\n", Method.getSpaces(3));
                    CallMethods.Append(Method.getSpaces(2) + "}\r\n");
                    continue;
                }
                if (c is IIGlobalMethod)
                {
                    loadMethod = c as IIGlobalMethod;
                    if (loadMethod.InPage(Html))
                    {
                        method = loadMethod.GetIGlobalMethod();
                        GetMethodsLines(method);
                        if (c is ReadTag)
                        {
                            ReadTag rd = c as ReadTag;
                            var inl = rd.getListMethods();
                            if (inl != null && inl.Count > 0)
                            {
                                foreach (var x in inl)
                                {
                                    x.InPageLoad = false;
                                    GetMethodsLines(x);
                                }
                            }
                        }
                    }
                }
                if (c is IGlobalField)
                {
                    field = (c as IGlobalField).GetGloabalField();
                    if (GloabalFileds.Contains(field))
                    {
                        continue;
                    }
                    GloabalFileds += field;
                }
            }
        }

        private void GetMethodsLines(Method method)
        {
            if (method != null)
            {
                if (method.InPageLoad)
                {
                    if (method.WillTestBeforLoad)
                    {
                        MethodLines.AppendFormat("{0}if ( {1} )\r\n", Method.getSpaces(3), method.GetTestBeforLoadStr());
                        MethodLines.Append(Method.getSpaces(4) + "{\r\n");
                        MethodLines.AppendFormat("{0}{1}();\r\n", Method.getSpaces(5), method.Name);
                        MethodLines.Append(Method.getSpaces(4) + "}\r\n");
                    }
                    else
                    {
                        MethodLines.AppendFormat("{0}{1}();\r\n", Method.getSpaces(4), method.Name);
                    }
                }
                MethodRects.Append(method.ToFullMethodRect());
            }
        }

        private void WriteTagGit()
        {
            TheMatch = Regex.Match(Html, "(?s)<!DOCTYPE[^>]*>(?-s)", RegexOptions.IgnoreCase);
            if (TheMatch.Success)
            {
                Html = Html.Replace(TheMatch.Value, string.Concat(TheMatch.Value, "\r\n<!-- Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home", " -->\r\n"));
            }
            else
            {
                Html = string.Concat("<!-- Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home", " -->\r\n", Html);
            }
        }

        /// <summary>
        /// 生成页面
        /// </summary>
        public void MakePage()
        {
            if (Config.convert)
            {
                this.ConverterTags();
                return;
            }
            string[] type = GetCodeTyep();
            string className = type[0] + "_" + Regex.Replace(this.PageName, @"\W", "_");
            string aspxFile = this.PageName + type[1];
            string codeFile = this.PageName + type[2];
            StringBuilder AspxCode = new StringBuilder();
            AspxCode.AppendFormat("<%@ {0} Language=\"C#\" AutoEventWireup=\"true\" CodeFile=\"{1}\" Inherits=\"{2}\"{3} {4} %>\r\n"
                , type[0], codeFile, className, this is IUC || this.ValidateRequest ? "" : " ValidateRequest=\"false\"", this is IUC || this.EnableViewState ? "" : "EnableViewState=\"false\"");
            AspxCode.AppendFormat("\r\n", codeFile, className);
            if (!(this is IUC))
            {
                WriteTagGit();
            }
            AspxCode.Append(this.GetAspxCode());
            FindAllMethodsOrFileds();
            StringBuilder AspxCsCode = new StringBuilder("using System;\r\n");
            AspxCsCode.Append("using System.Linq;\r\n");
            AspxCsCode.Append("using Tag.Vows.Web;\r\n");
            if (!string.IsNullOrEmpty(Config.dbNameSpace))
            {
                AspxCsCode.AppendFormat("using {0};\r\n", Config.dbNameSpace);
            }
            if (this.TagCallBack == "json" || this.TagCallBack == "form")
            {
                AspxCsCode.AppendFormat("using Newtonsoft.Json;\r\n", Config.dbNameSpace);
                AspxCsCode.AppendFormat("using System.Dynamic;\r\n", Config.dbNameSpace);
            }
            AspxCsCode.Append("\r\n");
            AspxCsCode.AppendFormat("//------------------------------------------------------------------------------\r\n");
            AspxCsCode.AppendFormat("// <auto-generated>\r\n");
            AspxCsCode.AppendFormat("//    此代码是根据模板生成的。\r\n");
            AspxCsCode.AppendFormat("//    生成时间: {0}。\r\n", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"));
            AspxCsCode.AppendFormat("//    应避免手动更改此文件。\r\n");
            AspxCsCode.AppendFormat("//    如果重新生成代码，则将覆盖对此文件的手动更改。\r\n");
            AspxCsCode.AppendFormat("// </auto-generated>\r\n");
            AspxCsCode.AppendFormat("//------------------------------------------------------------------------------\r\n");
            AspxCsCode.AppendFormat("/*  Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home  */\r\n\r\n");
            AspxCsCode.AppendFormat("public partial class {0} : {1}\r\n", className, type[3]);
            AspxCsCode.Append("{\r\n");
            AspxCsCode.Append(GloabalFileds);
            AspxCsCode.Append(Method.Space + "protected void Page_Load(object sender, EventArgs e)\r\n");
            AspxCsCode.Append(Method.Space + "{\r\n");
            AspxCsCode.AppendFormat("{0}if (!this.IsPostBack)\r\n", Method.getSpaces(2));
            AspxCsCode.Append(Method.getSpaces(2) + "{\r\n");
            AspxCsCode.AppendFormat("{0}if (this.Befor_Load_Tags())\r\n", Method.getSpaces(3));
            AspxCsCode.Append(Method.getSpaces(3) + "{\r\n");
            AspxCsCode.AppendFormat("{0}", MethodLines);
            AspxCsCode.Append(Method.getSpaces(3) + "}\r\n");
            AspxCsCode.Append(Method.getSpaces(2) + "}\r\n");
            AspxCsCode.Append(Method.Space + "}\r\n\r\n");
            AspxCsCode.Append(MethodRects);
            if (this.TagCallBack == "json" || this.TagCallBack == "form")
            {
                AspxCsCode.Append(Method.Space + "public override CallbackResult TagCallback()\r\n");
                AspxCsCode.Append(Method.Space + "{\r\n");
                AspxCsCode.AppendFormat("{0}CallbackResult callback = null;\r\n", Method.getSpaces(2));
                AspxCsCode.Append(CallMethods);
                AspxCsCode.AppendFormat("{0}return callback;\r\n", Method.getSpaces(2));
                AspxCsCode.Append(Method.Space + "}\r\n");
            }
            AspxCsCode.Append("\r\n");
            AspxCsCode.Append(this.Config.GetDbContext(this));
            AspxCsCode.Append("\r\n");
            AspxCsCode.Append(Method.Space + "protected override object GetDbObject()\r\n");
            AspxCsCode.Append(Method.Space + "{\r\n");
            AspxCsCode.AppendFormat("{0}return this.Db_Context;\r\n", Method.getSpaces(2));
            AspxCsCode.Append(Method.Space + "}\r\n");
            AspxCsCode.Append("}");
            string msg = "";
            string text = Regex.Replace(AspxCode.ToString(), @"(?:[\r\n]\s*[\r\n]){3,}", "\r\n");
            text = Regex.Replace(text, @"<!--\s*-->[\r\n]*", "");
            if (!Config.WriteTagPage(aspxFile, text, out msg))
            {
                this.Msg += msg;
            }
            if (!Config.WriteTagPage(codeFile, AspxCsCode.ToString(), out msg))
            {
                this.Msg += msg;
            }
        }

        protected string[] GetCodeTyep()
        {
            if (this is LabelPage)
            {
                if (this.SubpageExtends != "SubControl")
                {
                    this.SubpageExtends = string.Concat(this.SubpageExtends, "\r\n       /*重新指定了UserControl页面处理类，请确保 ",
                        this.SubpageExtends, " 直接或间接继承自'Tag.Vows.Web.SubControl'*/");
                }
            }
            else if (this is BasePage)
            {
                if (this.Extends != "TagPage")
                {
                    this.Extends = string.Concat(this.Extends, "\r\n       /*重新指定了页面处理类，请确保 ",
                        this.Extends, " 直接或间接继承自'Tag.Vows.Web.TagPage'*/");
                }
            }

            string[] type = new string[] { "Page", ".aspx", ".aspx.cs", this.Extends };
            if (this is IUC)
            {
                type[0] = "Control";
                type[1] = ".ascx";
                type[2] = ".ascx.cs";
                type[3] = this.SubpageExtends;
            }
            return type;
        }

        public string ToPageString()
        {
            if (TagList.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                Random ran = new Random(DateTime.Now.Second);
                sb.AppendFormat("<ul style='border:1px solid #{0}'>", GetRandomColor(ran));
                foreach (var c in TagList)
                {
                    sb.AppendFormat("<li style='border:1px solid #{0};margin-left:{1}px;'>{2}</li>", GetRandomColor(ran), Deep * 15, c.ToTagString());
                }
                sb.Append("</ul>");
                return sb.ToString();
            }
            else
            {
                return "<div>此页面无标签</div>";
            }
        }


        private string GetRandomColor(Random ran)
        {
            return "" + colors[ran.Next(15)] + colors[ran.Next(15)] + colors[ran.Next(15)] + colors[ran.Next(15)] + colors[ran.Next(15)] + colors[ran.Next(15)];
        }

        public string GetPageName()
        {
            return this.PageName;
        }

        public void ConverterTags()
        {
            foreach (var c in this.TagList)
            {
                Html = Html.Replace(c.Text, c.ConvertTagPair());
                this.Msg += c.GetMsg();
            }
            ConvertIfAndEmptyTags();
            string msg = "";
            bool success = false;
            if (this is StaticPage)
            {
                success = Config.ConvertStaticHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
            }
            else if (this is LabelPage)
            {
                success = Config.ConvertLabelHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
            }
            else if (this is SubListPage)
            {
                success = Config.ConvertItemHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
            }
            else if (this is ItemPage)
            {
                if (this.PageName == ListTag.FakeItemStr)
                {
                    success = true;
                }
                else
                {
                    success = Config.ConvertItemHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
                }
            }
            else
            {
                success = Config.ConvertPageHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
            }
            if (!success)
            {
                this.Msg += msg;
            }
        }

        public void ConvertIfAndEmptyTags()
        {
            this.Html = Regex.Replace(this.Html, Config.tagLeft + @"(?=\s*if\s*\?.+?)", Config.convert_pairs[0]);
            this.Html = Regex.Replace(this.Html, Config.tagLeft + @"(?=\s*el(?:se)?if\s*\?.+?)", Config.convert_pairs[0]);
            this.Html = Regex.Replace(this.Html, Config.tagLeft + @"(?=\s*else\s*)", Config.convert_pairs[0]);
            this.Html = Regex.Replace(this.Html, Config.tagLeft + @"(?=\s*/?\s*endif\s*)", Config.convert_pairs[0]);
            this.Html = Regex.Replace(this.Html, @"(?=<\s*if\s*\?.+?)" + Config.tagRight, Config.convert_pairs[1]);
            this.Html = Regex.Replace(this.Html, @"(?=<\s*el(?:se)?if\s*\?.+?)" + Config.tagRight, Config.convert_pairs[1]);
            this.Html = Regex.Replace(this.Html, @"(?=<\s*else\s*)" + Config.tagRight, Config.convert_pairs[1]);
            this.Html = Regex.Replace(this.Html, @"(?=<\s*/?\s*endif\s*)" + Config.tagRight, Config.convert_pairs[1]);
            this.Html = Regex.Replace(this.Html, Config.tagLeft + @"(?=\s*/?\s*empty\s*)", Config.convert_pairs[0]);
            this.Html = Regex.Replace(this.Html, @"(?=<\s*/?\s*empty\s*)" + Config.tagRight, Config.convert_pairs[1]);
        }
    }
}
