namespace Tag.Vows
{

    abstract class StyleAbleTag : BaseTag, IStyleAble
    {
        protected string Style { get; private set; }

        protected StyleAbleTag(string mtext, string origin, int mdeep, mPaths path, int no_)
            : base(mtext, origin, mdeep, path, no_)
        {

        }
        public void SetStyle(string style)
        {
            this.Style = style;
        }


        public bool HasStyle()
        {
            return !string.IsNullOrEmpty(this.Style);
        }

        public string GetStyle()
        {
            return this.Style;
        }
    }
}
