﻿using System;
using Tag.Vows.Interface;
using Tag.Vows.Bean;
using Tag.Vows.Page;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class LabelTag : BaseTag, IGlobalMethod, DeepLoadAble, ISubAble
    {
        public string LabeName;
        private Method loadAscx;
        private string ParPageName;
        protected new IHtmlAble SubPage;
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

        public Method GetGloabalMethod()
        {

            if (this.SubPage != null && loadAscx == null)
            {
                loadAscx = new Method();
                loadAscx.name = "Load_" + this.TagName;
                loadAscx.in_page_load = true;
                if (ParPageName == this.LabeName)
                {
                    loadAscx.body.AppendFormat("{0}/*（--未加载套用自己的标签--{1}）*/\r\n", Method.getSpaces(2), this.LabeName);
                }
                else
                {
                    loadAscx.body.AppendFormat("{0}SubControl uc_label=(SubControl) LoadControl( \"{1}.ascx\");\r\n", Method.getSpaces(2), this.LabeName);
                    loadAscx.body.AppendFormat("{0}uc_label.SetDb(db);\r\n", Method.getSpaces(2));
                    loadAscx.body.AppendFormat("{0}uc_label.SetConfig(this.config);\r\n", Method.getSpaces(2));
                    loadAscx.body.AppendFormat("{0}{1}.Controls.Add(uc_label);\r\n", Method.getSpaces(2), this.GetTagName());
                }
            }
            return loadAscx;
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
    }
}