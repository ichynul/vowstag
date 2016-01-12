
using System.Collections.Generic;
using System;

namespace Tag.Vows
{
    class JsonTag : BaseTag, ITableUseable, ICallBackAble
    {
        private string BaseParams;
        public string DataName;
        public List<FromVar> Vars = new List<FromVar>();
        private Method CallBack;
        private string ModType;
        public JsonTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {

        }

        public override string getCodeForAspx()
        {
            return this.getTagName();
        }

        protected override void Discover()
        {
            this.DataName = this.path.tagregex.getDataName(this.Text);
            this.BaseParams = this.path.tagregex.getBaseParams(this.Text);
            if (string.IsNullOrEmpty(this.BaseParams))
            {
                this.BaseParams = "pagesize = 99";
            }
        }

        public override string toTagString()
        {
            return "【全局名称" + this.getTagName() + ",标签类型：json，数据源名称：" + this.DataName + "，数据参数：" + this.BaseParams + "】<br />";
        }

        public string getPageName()
        {
            return this.DataName;
        }

        public bool CheckDataUseable()
        {
            return this.path.TableUseable(this.DataName);
        }

        public string TabledisAbledMsg()
        {
            return string.Format("数据表{0}已被设置为不可用，您无权操作该表！", this.DataName);
        }

        public Method getCallMethod()
        {
            if (CallBack == null)
            {
                CallBack = new Method();
                CallBack.name = "CallBack_" + this.getTagName();
                CallBack.returnType = "CallBackResult";
                CallBack.in_page_load = false;
                if (!CheckDataUseable())
                {
                    CallBack.body.AppendFormat("{0}/*{1}*/\r\n", Method.getSpaces(2), this.TabledisAbledMsg());
                }
                else
                {
                    CallBack.body.Append(TempleHelper.getTempleHelper(this.path).linq_getJson(this.DataName, this.BaseParams, out ModType, this.getTagName()));
                }
            }
            return CallBack;
        }
    }
}
