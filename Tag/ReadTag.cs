using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Tag.Vows
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
        public ReadTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {

        }
        public override string getCodeForAspx()
        {
            if (this.HasStyle())
            {
                return Regex.Replace(this.SubPage.getAspxCode(), @"(?<=<%.*?)\bread(?=\.\w+.*?%>)", this.tagName, RegexOptions.IgnoreCase);
            }
            return string.Format("<!-- {0} -->", this.tagName); ;
        }

        protected override void Discover()
        {
            this.DataName = this.path.tagregex.getDataName(this.Text);
            this.BaseParams = this.path.tagregex.getBaseParams(this.Text);
            if (string.IsNullOrEmpty(this.BaseParams))
            {
                this.BaseParams = "desc = true";
            }
            this.ModType = TempleHelper.getTempleHelper(this.path).getTableName(DataName);
        }

        public override string toTagString()
        {
            return "【全局名称" + this.getTagName() + ",标签类型：read，数据源名称：" + this.DataName + "，数据参数：" + this.BaseParams + "】<br />";
        }

        public Method getGloabalMethod()
        {
            if (ReadData == null)
            {
                ReadData = new Method();
                ReadData.name = "Bind_" + this.getTagName();
                ReadData.in_page_load = true;
                if (!CheckDataUseable())
                {
                    ReadData.body.AppendFormat("{0}/*{1}*/\r\n", Method.getSpaces(2), this.TabledisAbledMsg());
                }
                else
                {
                    ReadData.body.Append(TempleHelper.getTempleHelper(this.path).linq_getRead(this.DataName, this.BaseParams, out modType, this.HasStyle() ? this.tagName : "read"));
                    if (this.HasStyle())
                    {
                        if (subLsitMethod != null)
                        {
                            foreach (var x in subLsitMethod)
                            {
                                ReadData.body.AppendFormat("{0}{1}({2});\r\n", Method.getSpaces(2), x.name, this.tagName);
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
                string field = TempleHelper.getTempleHelper(this.path).getTableName(DataName);
                foreach (Method x in subLsitMethod)
                {
                    x.parsmstr = string.Concat(ModType, " read");
                    x.use_parsm = this.tagName;
                }
            }
            return subLsitMethod;
        }

        public string getGloabalField()
        {
            if (HasStyle())
            {
                return string.Format("{0}protected {1} {2};\r\n", Method.getSpaces(1), ModType, this.tagName);
            }
            return string.Format("{0}protected {1} read;\r\n", Method.getSpaces(1), ModType);
        }

        public string getNewReadName()
        {
            return string.Concat("read_", this.tagName);
        }

        public bool CheckDataUseable()
        {
            return this.path.TableUseable(this.DataName);
        }

        public string TabledisAbledMsg()
        {
            return string.Format("数据表{0}已被设置为不可用，您无权操作该表！", this.DataName);
        }

        public void LoadSubPage()
        {
            this.SubPage = new ReadPage(this.Style, Deep, this.path);
            this.SubPage.setUpDataName(this.DataName, FieldType.readValue);
            this.subLsitMethod = this.SubPage.getListMethods(this.DataName);
        }
    }
}