
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace Tag.Vows
{
    class CMDTag : BaseTag
    {
        private string BaseParams;
        private Dictionary<string, string> _cmdParams;

        public CMDTag(string mtext,string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {

        }

        public override string getCodeForAspx()
        {
            return string.Format("<!-- {0} -->", this.tagName);
        }

        protected override void Discover()
        {
            BaseParams = this.path.tagregex.getBaseParams(this.Text);
            _cmdParams = new Dictionary<string, string>();
            if (BaseParams.IndexOf("&") != -1)
            {
                string[] arr = BaseParams.Split('&');

                foreach (var kv in arr)
                {
                    string[] v = kv.Split('=');
                    if (v.Length == 2)
                    {
                        _cmdParams.Add(v[0].ToLower(), v[1]);
                    }
                }
            }
            else
            {
                string[] v = BaseParams.Split('=');
                if (v.Length == 2)
                {
                    _cmdParams.Add(v[0].ToLower(), v[1]);
                }
            }
        }

        public override string toTagString()
        {
            return "【全局名称" + this.getTagName() + ",标签类型：cmd，内容：" + this.BaseParams + "】<br />";
        }

        public string QueryString(string key)
        {
            string k = key.ToLower();
            if (_cmdParams != null && _cmdParams.Keys.Contains(k))
            {
                return _cmdParams[k];
            }
            return null;
        }
    }
}
