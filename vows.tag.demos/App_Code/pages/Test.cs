using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using model;
using xx.yy;

/// <summary>
///Test 的摘要说明
/// </summary>
public class Test : PageBase
{
    public Test()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    protected IQueryable<Article> xxoo(string kwd, int m)
    {
        return __db.Article.Where(x => x.Categ == m && x.Title.Contains(kwd));
    }

    protected string xx(string s)
    {
        return "xx_" + s;
    }
}