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
using Tag.Vows.Bean;
using Tag.Vows.Interface;
using Tag.Vows.Page;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class LabelTag : BaseTag, IIGlobalMethod, IDeepLoadAble, ISubAble
        , ITesBeforLoading
    {
        public string LabeName;
        private Method LoadAscx;
        private string ParPageName;
        protected new IHtmlAble SubPage;
        private bool TestBeforLoad;
        private HashSet<string> BeforLoadTests;

        public LabelTag(string mtext, string mOrigin, int Deep, string mParPageName, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {
            this.ParPageName = mParPageName;
        }

        protected override void Discover()
        {
            this.LabeName = this.Config.tagregex.getDataName(this.Text);
        }

        protected override string GetCodeForAspx()
        {
            if (ParPageName == this.LabeName)
            {
                return string.Format("<!--（未加载套用自己的label标签）。{0}-->", this.Text);
            }
            return string.Format("<asp:PlaceHolder ID=\"{0}\" runat=\"server\"></asp:PlaceHolder>", this.GetTagName());
        }

        /// <summary>
        /// 获取占位名称
        /// </summary>
        /// <returns></returns>
        public string GetPlaceholderName()
        {
            return this.PlaceHolderName;
        }

        public Method GetIGlobalMethod()
        {

            if (this.SubPage != null && LoadAscx == null)
            {
                LoadAscx = new Method();
                LoadAscx.Name = "Load_" + this.TagName;
                LoadAscx.InPageLoad = true;
                LoadAscx.WillTestBeforLoad = this.TestBeforLoad;
                LoadAscx.SetTestBeforLoad(this.BeforLoadTests);

                if (ParPageName == this.LabeName)
                {
                    LoadAscx.Body.AppendFormat("{0}/*（--未加载套用自己的标签--{1}）*/\r\n", Method.getSpaces(2), this.LabeName);
                }
                else
                {
                    LoadAscx.Body.AppendFormat("{0}SubControl uc_label=(SubControl) LoadControl( \"{1}.ascx\");\r\n", Method.getSpaces(2), this.LabeName);
                    LoadAscx.Body.AppendFormat("{0}uc_label.SetDb(this.Db_Context);\r\n", Method.getSpaces(2));
                    LoadAscx.Body.AppendFormat("{0}uc_label.SetConfig(this.config);\r\n", Method.getSpaces(2));
                    LoadAscx.Body.AppendFormat("{0}{1}.Controls.Add(uc_label);\r\n", Method.getSpaces(2), this.GetTagName());
                }
            }
            return LoadAscx;
        }

        public void LazyLoad()
        {
            if (ParPageName == this.LabeName)
            {
                this.Msg += string.Format("未加载套用自己的label标签--{0}<br />", this.LabeName);
                return;
            }
            if (this.LabeName != null)
            {
                this.LoadSubPage();
                if (SubPage != null)
                {
                    this.SubPage.MakePage();
                }
            }
            else
            {
                this.Msg += "未指定Label文件名称";
            }
        }

        public void LoadSubPage()
        {
            this.SubPage = new LabelPage(this.Config.LabelPath, (this as LabelTag).LabeName, Deep, this.Config);
        }

        public override string ToTagString()
        {
            string s = "【全局名称" + this.GetTagName() + ",标签类型：label，标签文件名：" + this.LabeName + "】<br />";
            if (this.SubPage != null)
            {
                s += "============子页面===========<br />" + this.SubPage.ToPageString() + "<br />";
                s += "============子页面完=========<br />";
            }
            return s;
        }

        public void SetTest(HashSet<string> link)
        {
            this.BeforLoadTests = link;
            this.TestBeforLoad = true;
        }
    }
}