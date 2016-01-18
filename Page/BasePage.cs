﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace Tag.Vows
{
    /// <summary>
    ///At 2015/07/15
    ///By ichynul@outlook.com
    /// </summary>
    public class BasePage : IHtmlAble
    {
        //
        protected mPaths Path;
        private string Entends;
        private string SubpageEntends = "SubControl";
        private bool? CallBack = null;
        private bool ValidateRequest = true;
        //
        private string TagCallBack = "";
        public string Html { get; protected set; }
        protected string HtmlpPath;
        public string PageName { get; private set; }
        protected string Msg = "";
        protected string AspxCode { get; private set; }
        protected string AspxCsCode { get; private set; }
        protected string ext = ".html";
        protected MatchCollection Matches;
        protected Match Match;
        protected StringBuilder MethodLines;
        protected StringBuilder MethodRects;
        protected StringBuilder CallMethods;
        protected string GloabalFileds = "";
        internal List<BaseTag> TagList = new List<BaseTag>();
        internal List<Method> MethodsInPage = new List<Method>();
        protected int Deep;

        protected BasePage() { }

        public BasePage(string mPageName, mPaths path)
            : this("", mPageName, 1, path)
        {
        }

        public BasePage(string mHtmlpPath, string mPageName, int mDeep, mPaths path)
        {
            path.Init();
            this.HtmlpPath = string.IsNullOrEmpty(mHtmlpPath) ? path.PagePath : mHtmlpPath;
            this.Path = path;
            this.Entends = path.defaultBase;
            this.Deep = mDeep + 1;

            this.PageName = mPageName;
            if (Deep > path.MAXDEEP)
            {
                this.Msg += string.Format("{0}-镶套层数达到{1}层，为防止循环套用已停止解析。<br />", this.PageName, path.MAXDEEP);
                return;
            }
            GetTegs(false);
        }

        public BasePage(string style, int mDeep, mPaths path, string fakeName)
        {
            this.Path = path;
            this.Entends = path.defaultBase;
            this.Deep = mDeep + 1;
            this.PageName = fakeName;
            this.Html = style;
            if (Deep > path.MAXDEEP)
            {
                this.Msg += string.Format("{0}-镶套层数达到{1}层，为防止循环套用已停止解析。<br />", this.PageName, path.MAXDEEP);
                return;
            }
            GetTegs(true);
        }

        public string GetMsg()
        {
            return this.Msg;
        }

        protected void GetTegs(bool style)
        {
            if (!style)
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
            if (!Path.allowServerScript)
            {
                DissAbleServerScript();
            }

            ReplaceSpacesAndMatchAll();
            if (!this.Path.convert)
            {
                RePathJsCssImg();
            }
            ReplaceEnd(false);
            FindListOrReadPairs(Html, 1);
            foreach (var x in this.TagList)
            {
                if (x is ListTag)
                {
                    (x as ListTag).LazyLoad();
                }
                this.Html = x.ReplaceTagText(this.Html);
            }
            FindIfPairs(Html);
            ReplaceEnd(true);
            //
            DoForForm();
            DoForPager();
            DoForRead();
        }

        private void DissAbleServerScript()
        {
            string value = "";
            string g = "";
            Matches = Regex.Matches(Html, this.Path.tagregex.ServerCodeTest, RegexOptions.IgnoreCase);
            for (int i = 0; i < Matches.Count; i += 1)
            {
                value = Matches[i].Value;
                g = Matches[i].Groups["code"].Value;
                Html = Html.Replace(value, value.Replace(g, string.Concat("\r\n    /*当前设置不允许服务端代码\r\n",
                    g, "\r\n    当前设置不允许服务端代码*/\r\n    ")));
            }
            Matches = Regex.Matches(Html, this.Path.tagregex.ServerScriptTest, RegexOptions.IgnoreCase);
            for (int i = 0; i < Matches.Count; i += 1)
            {
                value = Matches[i].Value;
                g = Matches[i].Groups["script"].Value;
                Html = Html.Replace(value, value.Replace(g, string.Concat("\r\n    /*当前设置不允许服务端代码\r\n",
                    g, "\r\n    当前设置不允许服务端代码*/\r\n    ")));
            }
        }

        private void ReplaceSpacesAndMatchAll()
        {
            Matches = Regex.Matches(Html, this.Path.tagregex.TagTest, RegexOptions.IgnoreCase);
            string tag = "", origin = "";
            for (int i = 0; i < Matches.Count; i += 1)
            {
                origin = Matches[i].Value;
                tag = Regex.Replace(origin, @"\s", string.Empty, RegexOptions.IgnoreCase);
                tag = Regex.Replace(tag, @"<br/>", "&", RegexOptions.IgnoreCase);
                tag = Regex.Replace(tag, @"<hr/>", "|", RegexOptions.IgnoreCase);
                //
                MatchTag(tag, origin, this.Path.tagregex.StaticTest, TagType._tag_static);
                MatchTag(tag, origin, this.Path.tagregex.FiledTest, TagType._tag_filed);
                MatchTag(tag, origin, this.Path.tagregex.MethodTest, TagType._tag_method);
                if (this is StaticPage)
                {
                    continue;
                }
                MatchTag(tag, origin, this.Path.tagregex.ListTest, TagType._tag_list);
                MatchTag(tag, origin, this.Path.tagregex.ReadTest, TagType._tag_read);
                MatchTag(tag, origin, this.Path.tagregex.LabelTest, TagType._tag_label);
                MatchTag(tag, origin, this.Path.tagregex.CommandTest, TagType._tag_command);
                if (this is LabelPage)
                {
                    continue;
                }
                if (!(this is SubListPage))
                {
                    MatchTag(tag, origin, this.Path.tagregex.PagerTest, TagType._tag_pager);
                }
                MatchTag(tag, origin, this.Path.tagregex.FormTest, TagType._tag_form);
                MatchTag(tag, origin, this.Path.tagregex.JsonTest, TagType._tag_json);
            }
        }

        private void MatchTag(string text, string origin, string pa, TagType type)
        {
            if (Regex.IsMatch(text, this.Path.tagregex.TagPairEndTest) || Regex.IsMatch(text, this.Path.tagregex.EmptyTest) || Regex.IsMatch(text, this.Path.tagregex.ifTagKeyTest))
            {
                return;
            }
            Match = Regex.Match(text, pa, RegexOptions.IgnoreCase);
            if (Match.Success)
            {
                this.Html = this.Html.Replace(origin, text);
                if (type == TagType._tag_command)
                {
                    CMDTag tag = new CMDTag(Match.Value, origin, Deep, this.Path, this.TagList.Count);
                    GetCMD(tag);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_list)
                {
                    if (this is SubListPage)
                    {
                        ListTag tag = new ListTag(Match.Value, origin, Deep, (this as SubListPage).PageName, this.Path, this.TagList.Count);
                        this.TagList.Add(tag);
                    }
                    else
                    {
                        ListTag tag = new ListTag(Match.Value, origin, Deep, this is ItemPage ? (this as ItemPage).PageName : "", this.Path, this.TagList.Count);
                        this.TagList.Add(tag);
                    }
                }
                else if (type == TagType._tag_read)
                {
                    ReadTag tag = new ReadTag(Match.Value, origin, Deep, this.Path, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_label)
                {
                    LabelTag tag = new LabelTag(Match.Value, origin, Deep, this is LabelPage ? (this as LabelPage).PageName : "", this.Path, this.TagList.Count);
                    tag.LazyLoad();
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_static)
                {
                    StaticTag tag = new StaticTag(Match.Value, origin, Deep, this is StaticPage ? (this as StaticPage).PageName : "", this.Path, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_filed)
                {
                    FieldTag tag = new FieldTag(Match.Value, origin, Deep, this.Path, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_method)
                {
                    MethodTag tag = new MethodTag(Match.Value, origin, Deep, this.Path, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_pager)
                {
                    PagerTag tag = new PagerTag(Match.Value, origin, Deep, this.Path, this.TagList.Count);
                    this.TagList.Add(tag);
                }
                else if (type == TagType._tag_form)
                {
                    FormTag tag = new FormTag(Match.Value, origin, Deep, this.Path, this.TagList.Count);
                    this.TagList.Add(tag);
                    this.TagCallBack = "form";
                }
                else if (type == TagType._tag_json)
                {
                    JsonTag tag = new JsonTag(Match.Value, origin, Deep, this.Path, this.TagList.Count);
                    this.TagList.Add(tag);
                    this.TagCallBack = "json";
                }
            }
        }

        protected void RePathJsCssImg()
        {
            Matches = Regex.Matches(Html, this.Path.tagregex.JsCssImageTest, RegexOptions.IgnoreCase);
            ReplaceLinks();
            Matches = Regex.Matches(Html, this.Path.tagregex.CssBgTest, RegexOptions.IgnoreCase);
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
                if (Regex.Match(src, this.Path.tagregex.ThisDirTest, RegexOptions.IgnoreCase).Success)
                {
                    Html = Html.Replace(src, string.Format("{0}{1}/{2}", Path.input.Replace("~", ""), this is LabelPage ? "label" :
                        this is StaticPage ? "static" : this is SubListPage ? "item" : "page", src));
                }
                else if (Regex.Match(src, this.Path.tagregex.OtherDirTest, RegexOptions.IgnoreCase).Success)
                {
                    Html = Html.Replace(src, string.Format("{0}{1}", Path.input.Replace("~", ""), src.Replace("../", "")));
                }
                r.Add(src);
            }
        }

        private void GetCMD(CMDTag cmd)
        {
            string value = cmd.QueryString("base");
            if (!string.IsNullOrEmpty(value))
            {
                if (this is LabelPage)
                {
                    this.SubpageEntends = value;
                }
                else if (this is BasePage)
                {
                    this.Entends = value;
                }
            }
            //
            value = cmd.QueryString("callback");
            this.CallBack = value != null
                && value.ToLower() == "true";
            //
            value = cmd.QueryString("validaterequest");
            this.ValidateRequest = value != null
                && value.ToLower() == "true";
        }

        private void DoForPager()
        {
            var tehTag = TagList.FirstOrDefault(a => a is PagerTag);
            if (tehTag != null)
            {
                foreach (var c in TagList.Where(x => x is ListTag))
                {
                    (c as ListTag).setPagerName((tehTag as PagerTag));
                }
            }
        }

        private void DoForRead()
        {
            var theTag = TagList.FirstOrDefault(a => a is ReadTag && !(a as ReadTag).HasStyle());
            if (theTag != null)
            {
                string dataName = (theTag as ReadTag).DataName;
                foreach (var c in TagList)
                {
                    if (c is FieldTag)
                    {
                        (c as FieldTag).SetDataName(dataName, FieldType.read_value);
                    }
                    else if (c is MethodTag)
                    {
                        (c as MethodTag).SetDataName(dataName, MethodType.read_value_method);
                    }
                    else if (c is ListTag)
                    {
                        (c as ListTag).SetUpperDataName(dataName, FieldType.read_value);
                    }
                }
            }
        }

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

        private void ReplaceEnd(bool includelistAndRead)
        {
            Matches = Regex.Matches(Html, this.Path.tagregex.TagPairEndTest);
            string name = "";
            foreach (Match m in Matches)
            {
                name = m.Groups["name"].Value.ToLower();

                if (Path.convert)
                {
                    Html = Html.Replace(m.Value, Path.convert_pairs[0] + name + Path.convert_pairs[1]);
                    continue;
                }
                if (!includelistAndRead && (name == "list" || name == "read" || name == "empty"))
                {
                    continue;
                }
                Html = Html.Replace(m.Value, string.Empty);
            }
        }

        private void FindListOrReadPairs(string text, int deep)
        {
            if (deep > 10)
            {
                return;
            }
            Matches = Regex.Matches(text, this.Path.tagregex.TagPairTest, RegexOptions.IgnoreCase);
            string style = "";
            string tag = "";
            IStyleAble st = null;
            foreach (Match m in Matches)
            {
                style = m.Groups["style"].Value;
                tag = m.Groups["tag"].Value;
                if (tag.ToLower() == "list" && Regex.Match(style, this.Path.tagregex.ListTest).Success)
                {
                    FindListOrReadPairs(style, deep + 1);
                    continue;
                }
                if (tag.ToLower() == "read" && Regex.Match(style, this.Path.tagregex.ReadTest).Success)
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

        private void FindIfPairs(string text)
        {
            Matches = Regex.Matches(text, this.Path.tagregex.IfPairTest, RegexOptions.IgnoreCase);
            foreach (Match m in Matches)
            {
                IfGroupTag ifGroup = new IfGroupTag(m.Value, this.Deep, this.Path, this.TagList.Count);
                this.TagList.Add(ifGroup);
                this.Html = this.Html.Replace(m.Value, ifGroup.GetPlaceholderName());
            }
            if (Matches.Count > 0)
            {
                FindIfPairs(this.Html);
            }
        }

        protected void RecoverIfGroupTags()
        {
            foreach (var c in this.TagList.Where(x => x is IfGroupTag))
            {
                this.Html = c.RecoverTagText(this.Html);
                this.Msg += c.GetMsg();
            }
        }

        protected void RecoverOtherTags()
        {
            ITableUseable ck = null;
            foreach (var c in this.TagList)
            {
                if (c is ITableUseable)
                {
                    ck = c as ITableUseable;
                    if (!ck.CheckDataUseable())
                    {
                        Html = Html.Replace(c.Text, string.Format("<!--{0}-->", ck.TabledisAbledMsg())) + "\r\n";
                        this.Msg += string.Concat(ck.TabledisAbledMsg(), "，在页面：", this.PageName, this.ext, "<br />");
                        continue;
                    }
                }
                Html = c.RecoverTagText(Html);
                this.Msg += c.GetMsg();
            }
        }

        public virtual string GetAspxCode()
        {
            this.RecoverIfGroupTags();
            this.RecoverOtherTags();
            if (!(this is IUC) && (this.TagCallBack == "json" || this.TagCallBack == "form" ||
                (this.CallBack == null && Path.creatScriptForAllPages) || this.CallBack == true))
            {
                Match = Regex.Match(Html, "</body>", RegexOptions.IgnoreCase);
                if (Match.Success)
                {
                    Match = Regex.Match(Html, "<form[^>]*>", RegexOptions.IgnoreCase);
                    if (Match.Success)
                    {
                        if (!Regex.IsMatch(Match.Value, @"runat=['""]server['""]", RegexOptions.IgnoreCase))
                        {
                            Html = Html.Replace(Match.Value, Regex.Replace(Match.Value, ">$",
                                " runat=\"server\">", RegexOptions.IgnoreCase));
                        }
                    }
                    else
                    {
                        Match = Regex.Match(Html, "</body>", RegexOptions.IgnoreCase);
                        if (Match.Success)
                        {
                            Html = Html.Replace(Match.Value, string.Concat("\r\n<form id=\"form1\" runat=\"server\"></form>\r\n", Match.Value));
                        }
                    }
                    Match = Regex.Match(Html, "</form>", RegexOptions.IgnoreCase);
                    if (Match.Success)
                    {
                        Html = Html.Replace("</form>", string.Concat(JsMaker.getCallBackJs().ToString(), "</form>"));
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

        protected void FindAllMenthodsOrFileds()
        {
            Method method = null;
            MethodLines = new StringBuilder();
            MethodRects = new StringBuilder();
            CallMethods = new StringBuilder();
            IGlobalMethod loadmethod = null;
            ICallBackAble call = null;
            string field = "";
            foreach (var c in this.TagList.OrderByDescending(x => x.Sort))
            {
                if (c is ICallBackAble)
                {
                    call = c as ICallBackAble;
                    method = call.GetCallMethod();
                    GetMethodsLines(method);
                    CallMethods.AppendFormat("{0}callBack = {1}();\r\n", Method.getSpaces(2), method.name);
                    CallMethods.AppendFormat("{0}if (callBack != null)\r\n", Method.getSpaces(2));
                    CallMethods.Append(Method.getSpaces(2) + "{\r\n");
                    CallMethods.AppendFormat("{0}return callBack;\r\n", Method.getSpaces(3));
                    CallMethods.Append(Method.getSpaces(2) + "}\r\n");
                    continue;
                }
                if (c is IGlobalMethod)
                {
                    loadmethod = c as IGlobalMethod;
                    method = loadmethod.GetGloabalMethod();
                    GetMethodsLines(method);
                    if (c is ReadTag)
                    {
                        ReadTag rd = c as ReadTag;
                        var inl = rd.getListMethods();
                        if (inl != null && inl.Count > 0)
                        {
                            foreach (var x in inl)
                            {
                                x.in_page_load = false;
                                GetMethodsLines(x);
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
                if (method.in_page_load)
                {
                    MethodLines.AppendFormat("{0}{1}();\r\n", Method.getSpaces(3), method.name);
                }
                MethodRects.Append(method.ToFullMethodRect());
            }
        }

        private void WriteTagGit()
        {
            Match = Regex.Match(Html, "(?s)<!DOCTYPE[^>]*>(?-s)", RegexOptions.IgnoreCase);
            if (Match.Success)
            {
                Html = Html.Replace(Match.Value, string.Concat(Match.Value, "\r\n<!-- Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home", " -->\r\n"));
            }
            else
            {
                Html = string.Concat("<!-- Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home", " -->\r\n", Html);
            }
        }

        public void MakePage()
        {
            if (Path.convert)
            {
                this.ConverterTags();
                return;
            }
            string[] type = GetCodeTyep();
            string className = type[0] + "_" + Regex.Replace(this.PageName, @"\W", "_");
            string aspxFile = this.PageName + type[1];
            string codeFile = this.PageName + type[2];
            StringBuilder AspxCode = new StringBuilder();
            AspxCode.AppendFormat("<%@ {0} Language=\"C#\" AutoEventWireup=\"true\" CodeFile=\"{1}\" Inherits=\"{2}\"{3} %>\r\n"
                , type[0], codeFile, className, this is IUC || !this.ValidateRequest ? "" : " ValidateRequest=\"true\"");
            AspxCode.AppendFormat("\r\n", codeFile, className);
            WriteTagGit();
            AspxCode.Append(this.GetAspxCode());
            FindAllMenthodsOrFileds();
            StringBuilder AspxCsCode = new StringBuilder("using System;\r\n");
            AspxCsCode.Append("using System.Linq;\r\n");
            AspxCsCode.Append("using Tag.Vows;\r\n");
            if (!string.IsNullOrEmpty(Path.dbNameSpace))
            {
                AspxCsCode.AppendFormat("using {0};\r\n", Path.dbNameSpace);
            }
            if (this.TagCallBack == "json" || this.TagCallBack == "form")
            {
                AspxCsCode.AppendFormat("using Newtonsoft.Json;\r\n", Path.dbNameSpace);
                AspxCsCode.AppendFormat("using System.Dynamic;\r\n", Path.dbNameSpace);
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
            AspxCsCode.Append(Method.space + "protected void Page_Load(object sender, EventArgs e)\r\n");
            AspxCsCode.Append(Method.space + "{\r\n");
            AspxCsCode.AppendFormat("{0}if (!this.IsPostBack)\r\n", Method.getSpaces(2));
            AspxCsCode.Append(Method.getSpaces(2) + "{\r\n");
            AspxCsCode.Append(MethodLines);
            AspxCsCode.Append(Method.getSpaces(2) + "}\r\n");
            AspxCsCode.Append(Method.space + "}\r\n\r\n");
            AspxCsCode.Append(MethodRects);
            if (this.TagCallBack == "json" || this.TagCallBack == "form")
            {
                AspxCsCode.Append(Method.space + "public override CallBackResult TagCallBack()\r\n");
                AspxCsCode.Append(Method.space + "{\r\n");
                AspxCsCode.AppendFormat("{0}CallBackResult callBack = null;\r\n", Method.getSpaces(2));
                AspxCsCode.Append(CallMethods);
                AspxCsCode.AppendFormat("{0}return callBack;\r\n", Method.getSpaces(2));
                AspxCsCode.Append(Method.space + "}\r\n");
            }
            AspxCsCode.Append("\r\n");
            AspxCsCode.Append(TempleHelper.getTempleHelper(this.Path).getDbcontex(this));
            AspxCsCode.Append("\r\n");
            AspxCsCode.Append(Method.space + "protected override object GetDbObject()\r\n");
            AspxCsCode.Append(Method.space + "{\r\n");
            AspxCsCode.AppendFormat("{0}return this.db;\r\n", Method.getSpaces(2));
            AspxCsCode.Append(Method.space + "}\r\n");
            AspxCsCode.Append("}");
            string msg = "";
            string text = Regex.Replace(AspxCode.ToString(), @"(?:[\r\n]\s*[\r\n]){3,}", "\r\n");
            text = Regex.Replace(text, @"<!--\s*-->[\r\n]*", "");
            if (!Path.WriteTagPage(aspxFile, text, out msg))
            {
                this.Msg += msg;
            }
            if (!Path.WriteTagPage(codeFile, AspxCsCode.ToString(), out msg))
            {
                this.Msg += msg;
            }
        }

        protected string[] GetCodeTyep()
        {
            if (this is LabelPage)
            {
                if (this.SubpageEntends != "SubControl")
                {
                    this.SubpageEntends = string.Concat(this.SubpageEntends, "\r\n       /*重新指定了UserControl页面处理类，请确保 ",
                        this.SubpageEntends, " 直接或间接继承自'Tag.Vows.SubControl'*/");
                }
            }
            else if (this is BasePage)
            {
                if (this.Entends != "TagPage")
                {
                    this.Entends = string.Concat(this.Entends, "\r\n       /*重新指定了页面处理类，请确保 ",
                        this.Entends, " 直接或间接继承自'Tag.Vows.TagPage'*/");
                }
            }

            string[] type = new string[] { "Page", ".aspx", ".aspx.cs", this.Entends };
            if (this is IUC)
            {
                type[0] = "Control";
                type[1] = ".ascx";
                type[2] = ".ascx.cs";
                type[3] = this.SubpageEntends;
            }
            return type;
        }

        public string ToPageString()
        {
            string spaces = MakeSpaces();
            string s = "";
            if (TagList.Count() > 0)
            {

                foreach (var c in TagList)
                {
                    s += c.ToTagString();
                }
                return Regex.Replace(s, @"(?s)<br/>|<br />|<br>(?-s)", "<br />" + spaces);
            }
            else
            {
                return spaces + "此页面无标签<br />";
            }
        }

        private string MakeSpaces()
        {
            return string.Format("<span style='margin-left:{0}px;'></span>", Deep * 15);
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
            Match = Regex.Match(Html, "(?s)<!DOCTYPE[^>]*>(?-s)", RegexOptions.IgnoreCase);
            if (Match.Success)
            {
                Html = Html.Replace(Match.Value, string.Concat(Match.Value, "\r\n<!-- 此页面是标签转换后的临时页面，请将其转移到对应文件夹。时间:"
                                , DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"), " -->\r\n"));
            }
            else
            {
                Html = string.Concat("<!-- 此页面是标签转换后的临时页面，请将其转移到对应文件夹。时间:",
                            DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"), " -->\r\n", Html);
            }
            string msg = "";
            bool success = false;
            if (this is StaticPage)
            {
                success = Path.ConvertStaticHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
            }
            else if (this is LabelPage)
            {
                success = Path.ConvertLabelHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
            }
            else if (this is SubListPage)
            {
                success = Path.ConvertItemHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
            }
            else if (this is ItemPage)
            {
                if (this.PageName == "x-item-fake-x")
                {
                    success = true;
                }
                else
                {
                    success = Path.ConvertItemHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
                }
            }
            else
            {
                success = Path.ConvertPageHtml(string.Concat(this.PageName, ".html"), this.Html, out msg);
            }
            if (!success)
            {
                this.Msg += msg;
            }
        }
    }
}