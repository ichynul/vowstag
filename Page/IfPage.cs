using System.Linq;

namespace Tag.Vows
{
    class IfPage : BasePage
    {
        private string test;
        public IfPage(string style, string test, int mDeep, mPaths path)
            : base(style, mDeep, path, "x-if-fake-x")
        {
        }
    }
}
