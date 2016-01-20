
using Tag.Vows.Interface;
using Tag.Vows.Tool;
namespace Tag.Vows.Page
{
    class LabelPage : BasePage, IUC
    {
        public LabelPage(string mHtmlpPath, string mPageName, int mDeep, TagConfig config)
            : base(mHtmlpPath, mPageName, mDeep, config)
        {

        }
    }
}