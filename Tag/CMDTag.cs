
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

        public CMDTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {

        }

        protected override string GetCodeForAspx()
        {
            return string.Format("<!-- {0} -->", this.TagName);
        }

        protected override void Discover()
        {
            BaseParams = this.Path.tagregex.getBaseParams(this.Text);
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
