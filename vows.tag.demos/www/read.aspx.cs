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

public partial class Page_read : xx.yy.Read
{
    protected Article read;
    protected Category ReadTag_30645_9;
    protected bool ListTag_55212_23Empty = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Befor_Load_Tags())
            {
                Bind_ReadTag_30882_5();
                Bind_ReadTag_30645_9();
                Bind_ListTag_71689_20();
                Bind_ListTag_55212_23();
                Load_LabelTag_16405_6();
            }
        }
    }

    public void Bind_ReadTag_30882_5()
    {
        /*id=request.id*/
        long long_id = long.MinValue;
        long.TryParse("" + Request.QueryString["id"],out long_id);
        read = Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID == long_id
                    )
                .FirstOrDefault();
        if (read == null)
        {
            read = new Article();
        }
    }

    public void Bind_ReadTag_30645_9()
    {
        /*id=read.categ*/
        int int_Categ_1 = read == null || !read.Categ.HasValue ? int.MinValue : read.Categ.Value;
;
        ReadTag_30645_9 = Db_Context.Category
                .Where( b=>
                     b.ID == int_Categ_1
                    )
                .FirstOrDefault();
        if (ReadTag_30645_9 == null)
        {
            ReadTag_30645_9 = new Category();
        }
    }

    public void Bind_ListTag_71689_20()
    {
        /*orderby=id&desc=true&id<request.id&emptytext=没有了&take=1*/
        long long_id = long.MinValue;
        long.TryParse("" + Request.QueryString["id"],out long_id);
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID < long_id
                    )
                    select new
                    {
                        a.ID,
                        a.Title
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(1);
        /*不分页，显示前 1条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_71689_20.Text = "<div class='emptydiv'><span class='emptytext'>没有了</span></div>";
            return;
        }
        ListTag_71689_20.DataSource = list.ToList();
        ListTag_71689_20.DataBind();
    }

    public void Bind_ListTag_55212_23()
    {
        /*orderby=id&desc=false&id>read.id&take=1*/
        long long_ID_3 = read == null ? long.MinValue: read.ID;
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID > long_ID_3
                    )
                    select new
                    {
                        a.ID,
                        a.Title
                    };
        int totalsize = list.Count();
        list = list.OrderBy(c=>c.ID).Take(1);
        /*不分页，显示前 1条数据;*/
        if (list.Count() == 0)
        {
            ListTag_55212_23Empty = true;
            return;
        }
        ListTag_55212_23.DataSource = list.ToList();
        ListTag_55212_23.DataBind();
    }

    public void Load_LabelTag_16405_6()
    {
        SubControl uc_label=(SubControl) LoadControl( "notice.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_16405_6.Controls.Add(uc_label);
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