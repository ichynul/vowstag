﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年05月09日 21:53:43。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag http://git.oschina.net/yuyunsichuang/vowstag/wikis/home  */

public partial class Page_listTag : xx.yy.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Befor_Load_Tags())
            {
                Bind_ListTag_26806_6();
                Bind_ListTag_41806_11();
                Bind_ListTag_54328_12();
                Bind_ListTag_77328_13();
                Bind_ListTag_67850_14();
                Bind_ListTag_76373_15();
                Bind_ListTag_82895_16();
                Bind_ListTag_22895_17();
                Load_LabelTag_16342_3();
            }
        }
    }

    public void Bind_ListTag_26806_6()
    {
        /*pagesize=3*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Skip((page - 1) * 3).Take(3);
        /*分页，每页显示3;*/
        string urlparams = RemovePageParams(Request.RawUrl);
        PagerTag_17806_10.Text = "<div id='pagerinfo' style='display:none;'>"
                + "\r\n<input type='hidden' id='list_size' value='" + totalsize + "' data-info='总纪录条数' />"
                + "\r\n<input type='hidden' id='num_per_page' value='" + 3 + "' data-info='每页显示' />"
                + "\r\n<input type='hidden' id='current_page' value='" + page + "' data-info='当前页' />"
                 + "\r\n<input type='hidden' id='current_url_params' value='" + urlparams + "' data-info='当前url参数（不带page参数）' />"
                + "\r\n</div>";
        if (list.Count() == 0)
        {
            empty_ListTag_26806_6.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_26806_6.DataSource = list.ToList();
        ListTag_26806_6.DataBind();
    }

    public void Bind_ListTag_41806_11()
    {
        /*take=1&item=listTag_item*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(1);
        /*不分页，显示前 1条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_41806_11.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_41806_11.DataSource = list.ToList();
        ListTag_41806_11.DataBind();
    }

    public void Bind_ListTag_54328_12()
    {
        /*id>=3&id<999&take=1&item=listTag_item*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID >= 3
                     && b.ID < 999
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(1);
        /*不分页，显示前 1条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_54328_12.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_54328_12.DataSource = list.ToList();
        ListTag_54328_12.DataBind();
    }

    public void Bind_ListTag_77328_13()
    {
        /*desc=false&id=3|id=6&take=5|(id<=15&id>12)&item=listTag_item*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID == 3
                     || b.ID == 6
                     || (b.ID <= 15
                     && b.ID > 12)
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        int totalsize = list.Count();
        list = list.OrderBy(c=>c.ID).Take(5);
        /*不分页，显示前 5条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_77328_13.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_77328_13.DataSource = list.ToList();
        ListTag_77328_13.DataBind();
    }

    public void Bind_ListTag_67850_14()
    {
        /*title%O&content!%"二 B"&title!%你好&item=listTag_item*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.Title.Contains("O")
                     && !b.Content.Contains("二 B")
                     && !b.Title.Contains("你好")
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        /*pagesize、take 都未指定，设置 take 为99;*/
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(99);
        /*不分页，显示前 99条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_67850_14.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_67850_14.DataSource = list.ToList();
        ListTag_67850_14.DataBind();
    }

    public void Bind_ListTag_76373_15()
    {
        /*time>2015/12/2512:25:54&time>DateTime.Now&item=listTag_item*/
        DateTime DateTime_Time_1 = DateTime.Parse("2015-12-25 12:25:54");
        DateTime DateTime_Time_2 = DateTime.Now;
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.Time > DateTime_Time_1
                     && b.Time > DateTime_Time_2
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        /*pagesize、take 都未指定，设置 take 为99;*/
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(99);
        /*不分页，显示前 99条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_76373_15.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_76373_15.DataSource = list.ToList();
        ListTag_76373_15.DataBind();
    }

    public void Bind_ListTag_82895_16()
    {
        /*uid=session.uid|uid=user.ID|author=cookie.uname&item=listTag_item*/
        long long_uid = long.MinValue;
        long.TryParse("" + Session["uid"],out long_uid);
        string string_uname = "" + Request.Cookies["uname"];
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.UID == long_uid
                     || b.UID == user.ID
                     || b.Author == string_uname
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        /*pagesize、take 都未指定，设置 take 为99;*/
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(99);
        /*不分页，显示前 99条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_82895_16.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_82895_16.DataSource = list.ToList();
        ListTag_82895_16.DataBind();
    }

    public void Bind_ListTag_22895_17()
    {
        /*数据表admin已被设置为不可用，您无权操作该表！*/
    }

    public void Load_LabelTag_16342_3()
    {
        SubControl uc_label=(SubControl) LoadControl( "notice.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_16342_3.Controls.Add(uc_label);
    }


    private Entities _Db_Context;
    protected Entities Db_Context
    {
        get
        {
            if (_Db_Context == null)
            {
                _Db_Context = new Entities();
            }
            return _Db_Context;
        }
    }

    protected override object GetDbObject()
    {
        return this.Db_Context;
    }
}