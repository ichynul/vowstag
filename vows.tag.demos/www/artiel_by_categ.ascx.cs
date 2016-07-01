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

public partial class Control_artiel_by_categ : SubControl
{
    protected Category item;
    public override void SetItem(object mItem)
    {
        this.item = mItem as Category;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Befor_Load_Tags())
            {
                Bind_ListTag_65390_3();
            }
        }
    }

    public void Bind_ListTag_65390_3()
    {
        /*categ=item.id&orderby=time,istop&desc=true&take=5*/
        long long_ID_1 = item == null ? long.MinValue: item.ID;
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.Categ == long_ID_1
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time,
                        a.Desc,
                        a.IsTop
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.Time)
                    .ThenByDescending(c=>c.IsTop).Take(5);
        /*不分页，显示前 5条数据;*/
        if (totalsize == 0)
        {
            empty_ListTag_65390_3.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_65390_3.DataSource = list.ToList();
        ListTag_65390_3.DataBind();
    }


    protected Entities Db_Context;
    public override void SetDb(object _db){
        this.Db_Context = _db as Entities;
    }

    protected override object GetDbObject()
    {
        return this.Db_Context;
    }
}