
using System.Collections.Generic;
using System;
using Tag.Vows.Interface;
using Tag.Vows.Web;
using Tag.Vows.Data;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class JsonTag : BaseTag, ITableUseable, ICallBackAble
    {
        private string BaseParams;
        public string DataName;
        public List<FromVar> Vars = new List<FromVar>();
        private Method CallBack;
        private string ModType;
        public JsonTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {

        }

        protected override string GetCodeForAspx()
        {
            return this.GetTagName();
        }

        protected override void Discover()
        {
            this.DataName = this.Config.tagregex.getDataName(this.Text);
            this.BaseParams = this.Config.tagregex.getBaseParams(this.Text);
            if (string.IsNullOrEmpty(this.BaseParams))
            {
                this.BaseParams = "pagesize = 99";
            }
        }

        public override string ToTagString()
        {
            return "【全局名称" + this.GetTagName() + ",标签类型：json，数据源名称：" + this.DataName + "，数据参数：" + this.BaseParams + "】<br />";
        }

        public string getPageName()
        {
            return this.DataName;
        }

        public bool CheckDataUseable()
        {
            return this.Config.TableUseable(this.DataName);
        }

        public string TabledisAbledMsg()
        {
            return string.Format("数据表{0}已被设置为不可用，您无权操作该表！", this.DataName);
        }

        public Method GetCallMethod()
        {
            if (CallBack == null)
            {
                CallBack = new Method();
                CallBack.name = "CallBack_" + this.GetTagName();
                CallBack.returnType = "CallBackResult";
                CallBack.in_page_load = false;
                if (!CheckDataUseable())
                {
                    CallBack.body.AppendFormat("{0}/*{1}*/\r\n", Method.getSpaces(2), this.TabledisAbledMsg());
                }
                else
                {
                    CallBack.body.Append(TempleHelper.getTempleHelper(this.Config).Linq_getJson(this.DataName, this.BaseParams, out ModType, this.GetTagName()));
                }
            }
            return CallBack;
        }
    }
}
