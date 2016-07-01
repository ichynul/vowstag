﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年07月01日 17:05:34。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag v-1.4.16.630 http://git.oschina.net/ichynul/vowstag */

public partial class Control_notice : SubControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Befor_Load_Tags())
            {
                Bind_ListTag_46336_1();
                Load_LabelTag_16476_4();
            }
        }
    }

    public void Bind_ListTag_46336_1()
    {
        /*istop=true&take=3&orderby=time*/
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.IsTop == true
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.Time).Take(3);
        /*不分页，显示前 3条数据;*/
        if (totalsize == 0)
        {
            empty_ListTag_46336_1.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_46336_1.DataSource = list.ToList();
        ListTag_46336_1.DataBind();
    }

    public void Load_LabelTag_16476_4()
    {
        SubControl uc_label=(SubControl) LoadControl( "another.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_16476_4.Controls.Add(uc_label);
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