using System.Text.RegularExpressions;

namespace Tag.Vows
{
    /// <summary>
    ///2015/07/15
    ///by lianghaiyun
    /// </summary>
    abstract class BaseTag : IConvertAble
    {
        public int Sort = 0;
        protected int NO_ = 0;
        protected mPaths path;
        protected string tagName;
        public bool In_Pairs { get; private set; }
        public string Text { get; protected set; }
        public string Origin { get; protected set; }
        protected string Msg = "";
        internal IMakeAble SubPage = null;
        protected int Deep;
        protected BaseTag() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mText">经过处理的标签文本（去除空格 \s*、br/hr替换）</param>
        /// <param name="mOrigin">未经过处理的标签原始文本</param>
        /// <param name="mDeep"></param>
        /// <param name="path"></param>
        /// <param name="no_"></param>
        protected BaseTag(string mText, string mOrigin, int mDeep, mPaths path, int no_)
        {
            this.path = path;
            this.Deep = mDeep;
            this.Text = mText;
            this.Origin = mOrigin;
            this.NO_ = no_;
            this.tagName = this.GetType().Name + "_" + this.Text.Length + "_" + (this.NO_ + 1);
            this.In_Pairs = this.Text.LastIndexOf('/') != this.Text.Length - 2;
            Discover();
        }

        /// <summary>
        /// 将标签文本替换成名称...{tagtext...} => tagname
        /// </summary>
        /// <param name="PageHtml"></param>
        /// <returns></returns>
        public string ReplaceTagText(string PageHtml)
        {
            string re = PageHtml.Replace(this.Text, this.tagName);
            return re;
        }

        /// <summary>
        /// 将标签成名称替换为经过处理的文本... tagname => tagcode
        /// </summary>
        /// <param name="PageHtml"></param>
        /// <returns></returns>
        public string RecoverTagText(string PageHtml)
        {
            string re = PageHtml.Replace(this.tagName, this.getCodeForAspx());
            return re;
        }

        /// <summary>
        /// 当执行标签对转换时调用
        /// </summary>
        /// <returns></returns>
        public string convertTagPair()
        {
            if (path.convert_pairs != null || path.convert_pairs.Length == 2)
            {
                this.Origin = Regex.Replace(this.Origin, "^" + path.tagLeft, path.convert_pairs[0]);
                this.Origin = Regex.Replace(this.Origin, path.tagRight + "$", path.convert_pairs[1]);
            }
            return this.Origin;
        }

        public string getTagName()
        {
            return this.tagName;
        }

        public string getMsg()
        {
            if (this.SubPage != null)
            {
                this.Msg += SubPage.getMsg();
            }
            return this.Msg;
        }
        /// <summary>
        /// 子类实现将标签转换为代码的具体逻辑
        /// </summary>
        /// <returns></returns>
        protected abstract string getCodeForAspx();
        /// <summary>
        /// 子类实现一些初始化工作
        /// </summary>
        protected abstract void Discover();
        /// <summary>
        /// 输出标签的内容，以供调试
        /// </summary>
        /// <returns></returns>
        public abstract string toTagString();
    }
}