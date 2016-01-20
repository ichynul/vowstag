using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using Tag.Vows.Interface;
using Tag.Vows.Tool;

namespace Tag.Vows.Web
{

    public abstract class SubControl : UserControl, IComTools
    {
        /// <summary>
        /// 用于保存站点及页面的通用信息
        /// </summary>
        protected dynamic config;
        /// <summary>
        /// callBack 时请求参数
        /// </summary>
        private int _page;
        private Tools _tools;

        public string TimeFormat(object time)
        {
            return tools.TimeFormat(time);
        }

        public string TimeFormat(object time, string format)
        {
            return tools.TimeFormat(time, format);
        }

        public string FloatFormat(object number, string format)
        {
            return tools.FloatFormat(number, format);
        }

        public string SubString(object str, int length)
        {
            return tools.SubString(str, length);
        }

        public string ValueOf(object obj)
        {
            return tools.ValueOf(obj);
        }

        protected int page
        {
            get
            {
                if (_page < 1)
                {
                    int.TryParse(Request.QueryString["page"], out _page);
                    _page = _page < 1 ? 1 : _page;
                }
                return _page;
            }
        }

        public string RemovePageParams(string url)
        {
            return tools.RemovePageParams(url);
        }

        internal Tools tools
        {
            get
            {
                if (_tools == null)
                {
                    _tools = new Tools();
                }
                return _tools;
            }
        }

        public void SetConfig(dynamic _config)
        {
            this.config = _config;
        }

        public virtual void SetItem(object mItem) { }

        public virtual void SetDb(object db) { }

        protected virtual object GetDbObject()
        {
            return null;
        }
    }
}