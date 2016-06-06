﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年06月02日 22:18:07。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home  */

public partial class Page_readTag : xx.yy.PageBase
{
    protected Article read;
    protected Article ReadTag_48112_17;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Befor_Load_Tags())
            {
                Bind_ReadTag_22125_4();
                Bind_ReadTag_48112_17();
                Bind_ListTag_68112_15();
                Load_LabelTag_16647_5();
            }
        }
    }

    public void Bind_ReadTag_22125_4()
    {
        /*id>10*/
        read = Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID > 10
                    )
                .FirstOrDefault();
        if (read == null)
        {
            read = new Article();
        }
    }

    public void Bind_ReadTag_48112_17()
    {
        /*orderby=id&desc=false&id>read.id*/
        long long_ID_3 = read == null ? long.MinValue: read.ID;
        ReadTag_48112_17 = Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID > long_ID_3
                    ).OrderBy(c=>c.ID)
                .FirstOrDefault();
        if (ReadTag_48112_17 == null)
        {
            ReadTag_48112_17 = new Article();
        }
    }

    public void Bind_ListTag_68112_15()
    {
        /*orderby=id&desc=true&id<read.id&emptytext=没有了&take=1*/
        long long_ID_3 = read == null ? long.MinValue: read.ID;
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID < long_ID_3
                    )
                    select new
                    {
                        a.Title,
                        a.ID
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(1);
        /*不分页，显示前 1条数据;*/
        if (list.Count() == 0)
        {
            empty_ListTag_68112_15.Text = "<div class='emptydiv'><span class='emptytext'>没有了</span></div>";
            return;
        }
        ListTag_68112_15.DataSource = list.ToList();
        ListTag_68112_15.DataBind();
    }

    public void Load_LabelTag_16647_5()
    {
        SubControl uc_label=(SubControl) LoadControl( "notice.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_16647_5.Controls.Add(uc_label);
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