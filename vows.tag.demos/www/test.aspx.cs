﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年06月02日 22:18:03。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home  */

public partial class Page_test : xx.yy.PageBase
{
    protected Article ReadTag_17809_5;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Befor_Load_Tags())
            {
                Bind_ReadTag_17809_5();
                Bind_ListTag_24809_1();
                Bind_ListTag_57809_9();
            }
        }
    }

    public void Bind_ReadTag_17809_5()
    {
        /*主键ID=4*/
        ReadTag_17809_5 = Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID == 4
                    )
                .FirstOrDefault();
        if (ReadTag_17809_5 == null)
        {
            ReadTag_17809_5 = new Article();
        }
    }

    public void Bind_ListTag_24809_1()
    {
        /*主键ID>3&take=3*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID > 3
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(3);
        /*不分页，显示前 3条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_24809_1.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_24809_1.DataSource = list.ToList();
        ListTag_24809_1.DataBind();
    }

    public void Bind_ListTag_57809_9()
    {
        /*id>3&skip=2&take=3&orderby=time&desc=true*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID > 3
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.Time).Skip(2).Take(3);
        /*不分页，显示前 3条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_57809_9.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_57809_9.DataSource = list.ToList();
        ListTag_57809_9.DataBind();
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