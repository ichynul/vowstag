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

        public abstract string getCodeForAspx();

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
        protected abstract void Discover();

        public abstract string toTagString();
    }
}