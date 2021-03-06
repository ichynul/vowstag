﻿#region  The MIT License (MIT)
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
using System.Text.RegularExpressions;
using Tag.Vows.Bean;
using Tag.Vows.Data;
using Tag.Vows.Enum;
using Tag.Vows.Interface;
using Tag.Vows.Page;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class ReadTag : StyleAbleTag, IGlobalField, IIGlobalMethod, ITableUseable
        , ISubAble, ITesBeforLoading, IUpperDataAble
    {
        protected new ReadPage SubPage;
        private string BaseParams;
        private Method ReadData;
        public string DataName;
        protected string UpDataname;
        private string ModType = "";
        private List<Method> subLsitMethod;
        private bool TestBeforLoad;
        private HashSet<string> BeforLoadTests;
        protected bool isSubRead;

        public ReadTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {
            this.Lev = 2;
        }

        protected override string GetCodeForAspx()
        {
            if (this.HasStyle())
            {
                return Regex.Replace(this.SubPage.GetAspxCode(), @"(?<=<%.*?)\bread(?=.*?%>)", this.TagName, RegexOptions.IgnoreCase);
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
            this.ModType = Helper.GetTableName(DataName);
        }

        public override string ToTagString()
        {
            return "【全局名称" + this.GetTagName() + ",标签类型：read，数据源名称：" + this.DataName + "，数据参数：" + this.BaseParams + "】<br />";
        }

        public Method GetIGlobalMethod()
        {
            if (ReadData == null)
            {
                ReadData = new Method();
                ReadData.Name = "Bind_" + this.GetTagName();
                ReadData.InPageLoad = true;
                ReadData.WillTestBeforLoad = this.TestBeforLoad;
                ReadData.SetTestBeforLoad(this.BeforLoadTests);
                if (!CheckDataUseable())
                {
                    ReadData.Body.AppendFormat("{0}/*{1}*/\r\n", Method.getSpaces(2), this.TabledisAbledMsg());
                }
                else
                {
                    ReadData.Body.Append(Helper.Linq_getRead(this.DataName, this.BaseParams, out ModType, UpDataname, this.HasStyle() ? this.TagName : "read"));
                    if (this.HasStyle())
                    {
                        if (subLsitMethod != null)
                        {
                            foreach (var x in subLsitMethod)
                            {
                                ReadData.Body.AppendFormat("{0}{1}({2});\r\n", Method.getSpaces(2), x.Name, this.TagName);
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
                    x.ParsmStr = string.Concat(ModType, " read");
                    x.UseParams = this.TagName;
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

        /// <summary>
        /// 获取占位名称
        /// </summary>
        /// <returns></returns>
        public string GetPlaceholderName()
        {
            return this.PlaceHolderName;
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
            this.subLsitMethod = this.SubPage.GetListMethods(this.DataName);
        }

        public void SetTest(HashSet<string> link)
        {
            this.BeforLoadTests = link;
            this.TestBeforLoad = true;
        }

        public void SetUpperDataName(string upDataName, FieldType type)
        {
            if (type == FieldType.read_value)
            {
                this.isSubRead = true;
            }
            this.UpDataname = upDataName;
        }
    }
}