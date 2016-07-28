using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tag.Vows.Interface;
using System.Text.RegularExpressions;
using Tag.Vows.Page;

namespace Tag.Vows.Tool
{
    class RePathTool : IRePath
    {
        private string inputPath;
        private Regex
            JsCssImageTest = new Regex(@"<(?:script|link|img).*?(?:src|href)=['""](?<src>(?:\.\./|\./)?[^/<>]+?/[^<>'""]+?\.(?:js|css|png|jpg|jpeg|gif|bmp|svg))['""]"
                              , RegexOptions.IgnoreCase | RegexOptions.Singleline),
            CssBgTest = new Regex(@"background(?:\-image)?\s*:.*?url\s*\(['""]?(?<src>(?:\.\./|\./)?[^/<>]+?/[^<>'""]+?\.(?:png|jpg|jpeg|gif|bmp|svg))\s*['""]?\)"
                              , RegexOptions.IgnoreCase | RegexOptions.Singleline),
            ThisDirTest = new Regex(@"^(?:\./)?[^/>'""]+/.*$", RegexOptions.IgnoreCase),
            OtherDirTest = new Regex(@"^\.\./[^/>'""]+/.*$", RegexOptions.IgnoreCase);
        private MatchCollection Matches;


        public void SetInputPath(string _inputPath)
        {
            inputPath = _inputPath;
        }

        public string RePathJsCssImg(string html, BasePage page)
        {
            Matches = JsCssImageTest.Matches(html);
            html = ReplaceLinks(html, page);
            Matches = CssBgTest.Matches(html);
            html = ReplaceLinks(html, page);
            return html;
        }

        private string ReplaceLinks(string html, BasePage page)
        {
            GroupCollection gc = null;
            string src = "";
            List<string> r = new List<string>();
            for (int i = 0; i < Matches.Count; i += 1)
            {
                gc = Matches[i].Groups;
                src = gc["src"].Value;
                if (string.IsNullOrEmpty(src) || r.Contains(src))
                {
                    continue;
                }
                if (src.Length > 6 && src.Substring(0, 6) == "../../")
                {
                    continue;
                }
                if (OtherDirTest.IsMatch(src))
                {
                    html = html.Replace(src, string.Format("{0}{1}", inputPath.Replace("~", ""), src.Replace("../", "")));
                }
                else if (ThisDirTest.IsMatch(src))
                {
                    if (src.Length > 2 && src.Substring(0, 2) == "./") //   ./xx/oo.js ==>xx/oo.js
                    {
                        html = html.Replace(src, src.Substring(2));
                        src = src.Substring(2);
                    }
                    html = html.Replace(src, string.Format("{0}{1}/{2}", inputPath.Replace("~", ""), page is LabelPage ? "label" :
                        page is StaticPage ? "static" : page is SubListPage ? "item" : "page", src));
                }

                r.Add(src);
            }
            return html;
        }
    }
}
