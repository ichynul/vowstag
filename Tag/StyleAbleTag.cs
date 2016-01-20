using Tag.Vows.Interface;
using Tag.Vows.Tool;
namespace Tag.Vows.Tag
{
    abstract class StyleAbleTag : BaseTag, IStyleAble
    {
        protected string Style { get; private set; }

        protected StyleAbleTag(string mtext, string origin, int mdeep, TagConfig config, int no_)
            : base(mtext, origin, mdeep, config, no_)
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
