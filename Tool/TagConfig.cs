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
using System.IO;
using System.Text;
using System.Web;
using Tag.Vows.Page;

namespace Tag.Vows.Tool
{
    public class TagConfig
    {
        internal TagRegex tagregex;
        /// <summary>
        /// 最大镶套层次，超过这个深度的自页面略过，防止循环镶套
        /// </summary>
        public int MAXDEEP = 10;
        /// <summary>
        /// 请设置输入目录的相对路径
        /// </summary>
        public string input;
        /// <summary>
        /// 受保护的表名，多个用|分割。
        /// </summary>
        public string protected_tables;
        /// <summary>
        /// Entities 类的命名空间，自动获取
        /// </summary>
        public string dbNameSpace { private set; get; }
        /// <summary>
        /// Entities 类的类名，自动获取
        /// </summary>
        public string entitiesName { private set; get; }
        /// <summary>
        /// 是否为所有页面生成callBack的js支持，默认为false。
        /// 若设置为true，则无论页面中是否有callbase 指令都为每一个页面生成js支持
        /// </summary>
        public bool creatScriptForAllPages = false;
        /// <summary>
        /// 是否允许直接书写服务端脚本（如 &lt;% somecode %&gt;&lt;script runat="server"&gt;somecode &lt;/script&gt;）。
        /// 出于安全考虑，默认不允许，建议在 cs代码中实现该逻辑的方法,然后用方法标签调用：{ dosome() }
        /// </summary>
        public bool allowServerScript = false;
        /// <summary>
        /// 所有页面默认默认处理类,默认为 Tag.Vows.TagPage
        /// </summary>
        public string DefaultBase = "TagPage";

        internal string PagePath { get; private set; }
        internal string LabelPath { get; private set; }
        internal string StaticlPath { get; private set; }
        internal string ItemPath { get; private set; }

        private object _db;
        private string _output;
        private string _absoUrlPath;
        /// <summary>
        /// 若要转换标签对（convert==true），则传入模板标签的 new string[2]{"left","right"};
        /// </summary>
        public string[] convert_pairs;
        /// <summary>
        /// 是否执行标签转换
        /// </summary>
        public bool convert;
        public string tagLeft { private set; get; }
        public string tagRight { private set; get; }

        internal bool Init()
        {
            if (tagregex == null)
            {
                PagePath = HttpContext.Current.Server.MapPath(input + "page/");
                LabelPath = HttpContext.Current.Server.MapPath(input + "label/");
                StaticlPath = HttpContext.Current.Server.MapPath(input + "static/");
                ItemPath = HttpContext.Current.Server.MapPath(input + "item/");
                tagregex = new TagRegex(this.tagLeft, this.tagRight);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 页面入口，需要页面结构信息
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="PageString">结构信息</param>
        /// <returns>页面错误信息</returns>
        public string MakePage(string pagename, out string PageString)
        {
            PageString = "";
            BasePage testpage = new BasePage(pagename, this);
            testpage.MakePage();
            PageString = testpage.ToPageString();
            return testpage.GetMsg();
        }

        /// <summary>
        /// 页面入口，不需要页面结构信息
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <returns>页面错误信息</returns>
        public string MakePage(string pagename)
        {
            BasePage testpage = new BasePage(pagename, this);
            testpage.MakePage();
            return testpage.GetMsg();
        }

        /// <summary>
        /// 生成页面时使用的标签对支持正则表达式
        /// 如new string[2]{"\{[@#$%]","\}"}，可匹配四种类型的标签对:
        /// {@.../}、{#.../}、{$.../}、{%.../}，不建议，这样的话可以同时使用不同风格的标签对。
        /// 也可只传入其中一种如 new string[2]{"\{@","\}"}。
        /// 某些情况下会引起正则表达式的混淆，则应进行\转义。
        /// </summary>
        public string[] current_pairs
        {
            set
            {
                this.tagLeft = value[0];
                this.tagRight = value[1];
            }
        }
        /// <summary>
        /// 请设置输出目录的相对路径
        /// </summary>
        public string output
        {
            get { return _output; }
            set
            {
                _output = HttpContext.Current.Server.MapPath(value);
                _absoUrlPath = value.Replace("~", "");
            }
        }

        /// <summary>
        /// 获取绝对输出路径
        /// </summary>
        /// <returns></returns>
        internal string GetOutputAbsoPath()
        {
            return _absoUrlPath;
        }

        /// <summary>
        /// 请传入Entities类的实例，
        /// </summary>
        public object db
        {
            get { return _db; }
            set
            {
                if (_db == null)
                {
                    _db = value;
                    entitiesName = _db.GetType().Name;
                    dbNameSpace = _db.GetType().Namespace;
                }
            }
        }

        internal bool TableUseable(string table)
        {
            if (string.IsNullOrEmpty(protected_tables))
            {
                return true;
            }
            return !string.IsNullOrEmpty(table) && !protected_tables.ToLower().Contains(table.ToLower());
        }

        internal bool WriteTagPage(string name, string text, out string msg)
        {
            return this.WriteFile(_output, name, text, out msg);
        }

        internal bool ConvertPageHtml(string name, string text, out string msg)
        {
            return this.WriteFile(PagePath + @"\convert\", name, text, out msg);
        }

        internal bool ConvertStaticHtml(string name, string text, out string msg)
        {
            return this.WriteFile(StaticlPath + @"\convert\", name, text, out msg);
        }

        internal bool ConvertLabelHtml(string name, string text, out string msg)
        {
            return this.WriteFile(LabelPath + @"\convert\", name, text, out msg);
        }

        internal bool ConvertItemHtml(string name, string text, out string msg)
        {
            return this.WriteFile(ItemPath + @"\convert\", name, text, out msg);
        }

        private bool WriteFile(string output, string name, string text, out string msg)
        {
            msg = "";
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }
            name = output + name;
            try
            {
                File.WriteAllText(name, text, Encoding.UTF8);
                return true;
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            return false;
        }
    }
}