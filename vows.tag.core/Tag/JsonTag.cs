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
using Tag.Vows.Data;
using Tag.Vows.Interface;
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
                CallBack.Name = "CallBack_" + this.GetTagName();
                CallBack.ReturnType = "CallBackResult";
                CallBack.InPageLoad = false;
                if (!CheckDataUseable())
                {
                    CallBack.Body.AppendFormat("{0}/*{1}*/\r\n", Method.getSpaces(2), this.TabledisAbledMsg());
                }
                else
                {
                    CallBack.Body.Append(TempleHelper.getTempleHelper(this.Config).Linq_getJson(this.DataName, this.BaseParams, out ModType, this.GetTagName()));
                }
            }
            return CallBack;
        }
    }
}
