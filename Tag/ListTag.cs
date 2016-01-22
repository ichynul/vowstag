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
using System.Text;
using System.Text.RegularExpressions;
using Tag.Vows.Interface;
using Tag.Vows.Enum;
using Tag.Vows.Web;
using Tag.Vows.Data;
using Tag.Vows.Page;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class ListTag : StyleAbleTag, IGlobalMethod, IDeepLoadAble, ISubAble
        , IGlobalField, IUpperDataAble, ITableUseable
    {
        protected new IHtmlAble SubPage;
        private string BaseParams;
        public string ItemName { get; private set; }
        public string DataName { get; private set; }
        private Method bindList_or_useAscx;
        private string ModType = "";
        private string UpModType = "";
        private string ParPageName;
        protected List<string> dataLists = new List<string>();
        protected string UpDataname;
        protected PagerTag Pger;
        private HashSet<string> ItemFields;
        public bool Inside_Read { get; private set; }
        public bool HasReadParams { get; private set; }
        protected bool isSublist;

        public ListTag(string mtext, string origin, int Deep, string mParPageName, TagConfig config, int no_)
            : base(mtext, origin, Deep, config, no_)
        {
            this.ParPageName = mParPageName;
        }

        public void SetUpperDataName(string upDataName, FieldType type)
        {
            if (type == FieldType.item_value)
            {
                this.isSublist = true;
            }
            this.UpDataname = upDataName;
        }

        public void setPagerName(PagerTag mPger)
        {
            this.Pger = mPger;
        }

        public void set_Inside_Read()
        {
            this.Inside_Read = true;
        }
        public bool HasSubList()
        {
            return this.SubPage == null ? false : this.SubPage is SubListPage;
        }

        protected override void Discover()
        {
            this.DataName = this.Config.tagregex.getDataName(this.Text);
            this.BaseParams = this.Config.tagregex.getBaseParams(this.Text);
            if (string.IsNullOrEmpty(this.BaseParams))
            {
                this.BaseParams = "take = 99";
            }
            this.HasReadParams = Regex.IsMatch(this.BaseParams, this.Config.tagregex.ReadValue);
            if (!this.In_Pairs)
            {
                this.ItemName = this.Config.tagregex.getItemPath(this.BaseParams);
            }
            else
            {
                this.ItemName = "x-item-fake-x";
            }
        }

        public void LazyLoad()
        {
            if (ParPageName == this.ItemName)
            {
                this.Msg += string.Format("未加载套用自己的list标签--{0}<br />", this.ItemName);
                return;
            }
            if (this.ItemName != null)
            {
                this.LoadSubPage();
                if (SubPage != null)
                {
                    if (SubPage is SubListPage)
                    {
                        var lp = this.SubPage as SubListPage;
                        lp.SetUpperDataName(this.DataName, FieldType.item_value);
                        this.SubPage.MakePage();
                    }
                    else
                    {
                        var lp = this.SubPage as ItemPage;
                        this.ItemFields = lp.GetItemFields();
                        if (this.HasStyle() && Config.convert)
                        {
                            lp.ConverterTags();
                        }
                    }
                }
            }
            else if (!this.HasStyle())
            {
                this.Msg += string.Concat("<b style='color:orange;'>",
                    this.Text, "<b/>：", "未指定item文件名称，list标签必须指定item样式（在参数中指定样式文件名'itemfilename'：",
                    " {list=xxx?..item=itemfilename../} 或 在标签对中指定'itemstyle'： {list=xxx?.....}itemstyle{/list} <br />");
            }
        }

        protected override string GetCodeForAspx()
        {
            if (this.ParPageName == this.ItemName)
            {
                return string.Format("<!--（未加载套用自己的list标签）。{0}-->", string.Concat(this.Text, "--", ParPageName, "---", ItemName));
            }
            if (!HasSubList())
            {
                return string.Format(
                    "\r\n<asp:Repeater ID=\"{0}\" runat=\"server\">" +
                    "\r\n    <ItemTemplate>{1}</ItemTemplate>" +
                    "\r\n</asp:Repeater>" +
                    "\r\n<asp:Literal ID=\"empty_{0}\" runat=\"server\"></asp:Literal>",
                    this.GetTagName(), this.SubPage.GetAspxCode());
            }
            else
            {
                return string.Format("<asp:PlaceHolder ID=\"{0}\" runat=\"server\"></asp:PlaceHolder>" +
                    "\r\n<asp:Literal ID=\"empty_{0}\" runat=\"server\"></asp:Literal>\r\n", this.GetTagName());
            }
        }

        public void LoadSubPage()
        {
            ItemPage itempage = null;
            if (HasStyle())
            {
                itempage = new ItemPage(this.Style, Deep, this.Config);
            }
            else
            {
                itempage = new ItemPage(this.Config.ItemPath, this.ItemName, Deep, this.Config);
            }
            this.SubPage = itempage.GetItemInstance();
        }


        public Method GetGloabalMethod()
        {
            bool in_page_load = false;
            if (this.HasReadParams)
            {
                in_page_load = Inside_Read ? false : true;
            }
            else
            {
                in_page_load = true;
            }
            if (this.SubPage != null && bindList_or_useAscx == null)
            {
                bindList_or_useAscx = new Method();
                bindList_or_useAscx.name = string.Concat("Bind_", this.GetTagName());
                bindList_or_useAscx.in_page_load = in_page_load;
                if (!CheckDataUseable())
                {
                    bindList_or_useAscx.body.AppendFormat("{0}/*{1}*/\r\n", Method.getSpaces(2), this.TabledisAbledMsg());
                }
                else if (ParPageName == this.ItemName)
                {
                    bindList_or_useAscx.body.AppendFormat("{0}/*（未加载套用自己的标签）。{1}*/\r\n", Method.getSpaces(2), this.TagName);
                }
                else
                {
                    bindList_or_useAscx.body.Append(TempleHelper.getTempleHelper(this.Config).Linq_getList(this.DataName, this.BaseParams,
                                                        this.ItemFields, out ModType, this.UpDataname, out UpModType, Pger));
                    bindList_or_useAscx.body.AppendFormat("{0}if (list.Count() == 0)\r\n", Method.getSpaces(2));
                    bindList_or_useAscx.body.Append(Method.getSpaces(2) + "{\r\n");
                    string emptytext = (this.SubPage as ItemPage).GetEmptyText();
                    if (!string.IsNullOrEmpty(emptytext))
                    {
                        bindList_or_useAscx.body.AppendFormat("{0}empty_{1}.Text = \"{2}\";\r\n", Method.getSpaces(3),
                                                            this.GetTagName(), Regex.Replace(emptytext, @"""", "\\\""));
                    }
                    else
                    {
                        emptytext = "暂无内容";
                        Match m = Regex.Match(BaseParams, @"(?<=emptytext=)[^&]+?(?=&|$)");
                        if (m.Success)
                        {
                            emptytext = m.Value;
                        }
                        if (emptytext != "none")
                        {
                            bindList_or_useAscx.body.AppendFormat("{0}empty_{1}.Text = \"<div class='emptydiv'><span class='emptytext'>{2}</span></div>\";\r\n",
                                Method.getSpaces(3), this.GetTagName(), emptytext);
                        }
                    }
                    bindList_or_useAscx.body.Append(Method.getSpaces(3) + "return;\r\n");
                    bindList_or_useAscx.body.Append(Method.getSpaces(2) + "}\r\n");
                    bindList_or_useAscx.body.Append(!this.HasSubList() ? BindRepeater() : BindPlaceHolder());
                }
            }
            return bindList_or_useAscx;
        }

        private string BindRepeater()
        {
            StringBuilder code = new StringBuilder();
            code.AppendFormat("{0}{1}.DataSource = list;\r\n", Method.getSpaces(2), this.GetTagName());
            code.AppendFormat("{0}{1}.DataBind();\r\n", Method.getSpaces(2), this.GetTagName());
            return code.ToString();
        }

        private string BindPlaceHolder()
        {
            StringBuilder code = new StringBuilder();
            code.AppendFormat("{0}foreach({1} item in list)\r\n", Method.getSpaces(2), ModType);
            code.Append(Method.getSpaces(2) + "{\r\n");
            code.AppendFormat("{0}SubControl uc_item=(SubControl) LoadControl( \"{1}.ascx\");\r\n", Method.getSpaces(3), this.SubPage.GetPageName());
            code.AppendFormat("{0}uc_item.SetItem(item);\r\n", Method.getSpaces(3));
            code.AppendFormat("{0}uc_item.SetDb(db);\r\n", Method.getSpaces(3));
            code.AppendFormat("{0}uc_item.SetConfig(this.config);\r\n", Method.getSpaces(3));
            code.AppendFormat("{0}{1}.Controls.Add(uc_item);\r\n", Method.getSpaces(3), this.GetTagName());
            code.Append(Method.getSpaces(2) + "}\r\n");
            return code.ToString();
        }

        public override string ToTagString()
        {
            string s = "【全局名称" + this.GetTagName() + ",标签类型：list，数据源名称：" +
                this.DataName + "，item文件名：" + this.ItemName + "，数据参数：" + this.BaseParams
                + (!HasSubList() ? "，item为简单页面" : "，item为复合页面") + "】<br />";
            if (this.SubPage != null)
            {
                s += "============子页面===========<br />" + this.SubPage.ToPageString();
                s += "============子页面完=========<br />";
            }
            return s;
        }

        public string GetGloabalField()
        {
            if (!this.isSublist || UpDataname == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}protected {1} item;\r\n", Method.getSpaces(1), UpModType);
            sb.AppendFormat("{0}public override void SetItem(object mItem)\r\n", Method.getSpaces(1));
            sb.Append(Method.getSpaces(1) + "{\r\n");
            sb.AppendFormat("{0}this.item = mItem as {1};\r\n", Method.getSpaces(2), UpModType);
            sb.Append(Method.getSpaces(1) + "}\r\n");
            return sb.ToString();
        }

        public bool CheckDataUseable()
        {
            return this.Config.TableUseable(this.DataName);
        }

        public string TabledisAbledMsg()
        {
            return string.Format("数据表{0}已被设置为不可用，您无权操作该表！", this.DataName);
        }
    }
}