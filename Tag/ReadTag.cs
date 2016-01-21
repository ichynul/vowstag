using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tag.Vows.Interface;
using Tag.Vows.Enum;
using Tag.Vows.Web;
using Tag.Vows.Data;
using Tag.Vows.Page;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class ReadTag : StyleAbleTag, IGlobalField, IGlobalMethod, ITableUseable, ISubAble
    {
        protected new ReadPage SubPage;
        private string BaseParams;
        private Method ReadData;
        public string DataName;
        private string ModType = "";
        private string modType;
        private List<Method> subLsitMethod;
        public ReadTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {

        }
        protected override string GetCodeForAspx()
        {
            if (this.HasStyle())
            {
                return Regex.Replace(this.SubPage.GetAspxCode(), @"(?<=<%.*?)\bread(?=\.\w+.*?%>)", this.TagName, RegexOptions.IgnoreCase);
            }
            return string.Format("<!-- {0} -->", this.TagName); ;
        }

        protected override void Discover()
        {
            this.DataName = this.Config.tagregex.getDataName(this.Text);
            this.BaseParams = this.Config.tagregex.getBaseParams(this.Text);
            if (string.IsNullOrEmpty(this.BaseParams))
            {
                this.BaseParams = "desc = true";
            }
            this.ModType = TempleHelper.getTempleHelper(this.Config).GetTableName(DataName);
        }

        public override string ToTagString()
        {
            return "【全局名称" + this.GetTagName() + ",标签类型：read，数据源名称：" + this.DataName + "，数据参数：" + this.BaseParams + "】<br />";
        }

        public Method GetGloabalMethod()
        {
            if (ReadData == null)
            {
                ReadData = new Method();
                ReadData.name = "Bind_" + this.GetTagName();
                ReadData.in_page_load = true;
                if (!CheckDataUseable())
                {
                    ReadData.body.AppendFormat("{0}/*{1}*/\r\n", Method.getSpaces(2), this.TabledisAbledMsg());
                }
                else
                {
                    ReadData.body.Append(TempleHelper.getTempleHelper(this.Config).Linq_getRead(this.DataName, this.BaseParams, out modType, this.HasStyle() ? this.TagName : "read"));
                    if (this.HasStyle())
                    {
                        if (subLsitMethod != null)
                        {
                            foreach (var x in subLsitMethod)
                            {
                                ReadData.body.AppendFormat("{0}{1}({2});\r\n", Method.getSpaces(2), x.name, this.TagName);
                            }
                        }
                    }
                }
            }
            return ReadData;
        }

        public List<Method> getListMethods()
        {
            if (this.HasStyle())
            {
                foreach (Method x in subLsitMethod)
                {
                    x.parsmstr = string.Concat(ModType, " read");
                    x.use_parsm = this.TagName;
                }
            }
            return subLsitMethod;
        }

        public string GetGloabalField()
        {
            if (HasStyle())
            {
                return string.Format("{0}protected {1} {2};\r\n", Method.getSpaces(1), ModType, this.TagName);
            }
            return string.Format("{0}protected {1} read;\r\n", Method.getSpaces(1), ModType);
        }

        public string getNewReadName()
        {
            return string.Concat("read_", this.TagName);
        }

        public bool CheckDataUseable()
        {
            return this.Config.TableUseable(this.DataName);
        }

        public string TabledisAbledMsg()
        {
            return string.Format("数据表{0}已被设置为不可用，您无权操作该表！", this.DataName);
        }

        public void LoadSubPage()
        {
            this.SubPage = new ReadPage(this.Style, Deep, this.Config);
            this.SubPage.SetUpperDataName(this.DataName, FieldType.read_value);
            this.subLsitMethod = this.SubPage.getListMethods(this.DataName);
        }
    }
}