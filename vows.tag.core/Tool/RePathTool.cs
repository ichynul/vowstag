#region  The MIT License (MIT)
/*
The MIT License (MIT)

Copyright (c) 2016 ichynul

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tag.Vows.Interface;
using Tag.Vows.Page;
using System.Linq;

namespace Tag.Vows.Tool
{
    class RePathTool : IRePath
    {
        private string inputPath;
        private List<string> baseDirs;
        private List<string> srcDirs;
        private Regex
            JsCssImageTest = new Regex(@"<(?:script|link|img).*?(?:src|href)=['""]\s*(?<src>[^<>'""]+?\.(?:js|css|png|jpg|jpeg|gif|bmp|svg))\s*['""]"
                              , RegexOptions.IgnoreCase | RegexOptions.Singleline),
            CssBgTest = new Regex(@"background(?:\-image)?\s*:.*?url\s*\(['""]?\s*(?<src>[^<>'""]+?\.(?:png|jpg|jpeg|gif|bmp|svg))\s*['""]?\)"
                              , RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private MatchCollection Matches;

        /// <summary>
        ///  RePathTool
        /// </summary>
        /// <param name="_inputPath"></param>
        public RePathTool(string _inputPath)
        {
            inputPath = _inputPath.Replace("~", string.Empty);
            baseDirs = inputPath.Split('/').Where(x => x.Length > 0).ToList();
        }

        /// <summary>
        /// 处理html中的资源src
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="page">page</param>
        /// <returns>html</returns>
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
                if (src[0] == '/')
                {
                    continue;
                }
                srcDirs = src.Split('/').Where(x => x.Length > 0).ToList();
                int n = srcDirs.Where(x => x == "..").Count();
                if (n == 0)
                {
                    html = html.Replace(src, string.Format("{0}{1}/{2}", inputPath,
                        page is LabelPage ? "label" : page is StaticPage ? "static" : page is SubListPage ? "item" : "page"
                        , srcDirs[0].Trim() == "." ? src.Trim().Substring(2) : src.Trim()));
                }
                else
                {
                    if (n == baseDirs.Count())
                    {
                        html = html.Replace(src, string.Format("/{0}/{1}", baseDirs[0], Regex.Replace(src.Trim(), @"(\.\.\/)+", string.Empty)));
                    }
                    else if (n < baseDirs.Count())
                    {
                        int d = baseDirs.Count() - n;
                        string s = "/";
                        for (int j = 0; j <= n; j += 1)
                        {
                            s += baseDirs[j] + "/";
                        }
                        html = html.Replace(src, string.Format("{0}{1}", s, Regex.Replace(src.Trim(), @"(\.\.\/)+", string.Empty)));
                    }
                }
                r.Add(src);
            }
            return html;
        }
    }
}
