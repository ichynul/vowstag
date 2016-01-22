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
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Specialized;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class CMDTag : BaseTag
    {
        private string BaseParams;
        private NameValueCollection _cmdParams;

        public CMDTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {

        }

        protected override string GetCodeForAspx()
        {
            return string.Format("<!-- {0} -->", this.TagName);
        }

        protected override void Discover()
        {
            BaseParams = this.Config.tagregex.getBaseParams(this.Text);
            _cmdParams = new NameValueCollection();
            if (!string.IsNullOrEmpty(this.BaseParams))
            {
                string[] arr = BaseParams.Split('&');
                foreach (var kv in arr)
                {
                    string[] v = kv.Split('=');
                    if (v.Length == 2)
                    {
                        _cmdParams.Add(v[0], v[1]);
                    }
                }
            }
        }

        public override string ToTagString()
        {
            return "【全局名称" + this.GetTagName() + ",标签类型：cmd，内容：" + this.BaseParams + "】<br />";
        }

        public string QueryString(string key)
        {
            string k = key.ToLower();
            return _cmdParams[k];
        }
    }
}
