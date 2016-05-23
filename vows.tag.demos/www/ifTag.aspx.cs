﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年05月22日 22:40:14。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag http://git.oschina.net/yuyunsichuang/vowstag/wikis/home  */

public partial class Page_ifTag : xx.yy.PageBase
{
    protected bool ListTag_26925_6Empty = false;
    protected bool ListTag_37925_10Empty = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Befor_Load_Tags())
            {
            if ( (Request.QueryString["kwd"] =="22")
                    && (Request.QueryString["kwd"] =="2")
                    && (Request.QueryString["xx"]=="oo") )
                {
                    Bind_ListTag_20925_4();
                }
                Bind_ListTag_26925_6();
                Bind_ListTag_37925_10();
            if ( (Request.QueryString["kwd"] =="22")
                    && (Request.QueryString["kwd"] =="2") )
                {
                    Load_LabelTag_15461_3();
                }
            }
        }
    }

    public void Bind_ListTag_20925_4()
    {
        /*id>1*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID > 1
                    )
                    select new
                    {
                        a.Title,
                        a.ID
                    };
        /*pagesize、take 都未指定，设置 take 为99;*/
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(99);
        /*不分页，显示前 99条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_20925_4.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_20925_4.DataSource = list.ToList();
        ListTag_20925_4.DataBind();
    }

    public void Bind_ListTag_26925_6()
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
        /*无法分页，未设置分页标签！;*/
        if (list.Count() == 0)
        {
            ListTag_26925_6Empty = true;
            return;
        }
        ListTag_26925_6.DataSource = list.ToList();
        ListTag_26925_6.DataBind();
    }

    public void Bind_ListTag_37925_10()
    {
        /*pagesize=3&desc=false*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        int totalsize = list.Count();
        list = list.OrderBy(c=>c.ID).Skip((page - 1) * 3).Take(3);
        /*分页，每页显示3;*/
        /*无法分页，未设置分页标签！;*/
        if (list.Count() == 0)
        {
            ListTag_37925_10Empty = true;
            return;
        }
        ListTag_37925_10.DataSource = list.ToList();
        ListTag_37925_10.DataBind();
    }

    public void Load_LabelTag_15461_3()
    {
        SubControl uc_label=(SubControl) LoadControl( "notice.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_15461_3.Controls.Add(uc_label);
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