﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年08月06日 16:44:55。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag v-1.4.16.727 http://git.oschina.net/ichynul/vowstag */

public partial class Page_index : xx.yy.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ClientScript.GetCallbackEventReference(this, "", "", "");
            if (this.Befor_Load_Tags())
            {
                Bind_ListTag_67862_5();
                Load_LabelTag_16833_3();
            }
        }
    }

    public void Bind_ListTag_67862_5()
    {
        /*orderby=id&desc=true&take=99&item=artiel_by_categ*/
        var list = from a in Db_Context.Category
                    select a;
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(99);
        /*不分页，显示前 99条数据;*/
        if (totalsize == 0)
        {
            empty_ListTag_67862_5.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        foreach(Category _item in list)
        {
            SubControl uc_item=(SubControl) LoadControl( "artiel_by_categ.ascx");
            uc_item.SetItem(_item);
            uc_item.SetDb(this.Db_Context);
            uc_item.SetConfig(this.config);
            ListTag_67862_5.Controls.Add(uc_item);
        }
    }

    public void Load_LabelTag_16833_3()
    {
        SubControl uc_label=(SubControl) LoadControl( "notice.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_16833_3.Controls.Add(uc_label);
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

    protected override void OnUnload(EventArgs e)
    {
        if (_Db_Context != null)
        {
            _Db_Context.Dispose();
        }
        base.OnUnload(e);
    }
}