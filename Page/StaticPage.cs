
using System.Text.RegularExpressions;
using Tag.Vows.Tool;
namespace Tag.Vows.Page
{
    class StaticPage : BasePage
    {
        public StaticPage(string mHtmlpPath, string mPageName, int mDeep, TagConfig config)
            : base(mHtmlpPath, mPageName, mDeep, config)
        {
        }
    }
}