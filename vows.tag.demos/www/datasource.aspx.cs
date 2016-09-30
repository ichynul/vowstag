﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年09月29日 09:30:34。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag v-1.4.16.8 http://git.oschina.net/ichynul/vowstag */

public partial class Page_datasource : Test
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ClientScript.GetCallbackEventReference(this, "", "", "");
            if (this.Befor_Load_Tags())
            {
                Bind_ListTag_39334_7();
                Bind_ListTag_31448_11();
                Load_LabelTag_1699_4();
            }
        }
    }

    public void Bind_ListTag_39334_7()
    {
        /*data=xxoo(request.xx,6)*/
        var list =from a in xxoo(Request.QueryString["xx"],6)
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
        if (totalsize == 0)
        {
            empty_ListTag_39334_7.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_39334_7.DataSource = list.ToList();
        ListTag_39334_7.DataBind();
    }

    public void Bind_ListTag_31448_11()
    {
        /*id>cookie.id.id*/
        long long_id_id = long.MinValue;
        long.TryParse(GetCookie("id","id"),out long_id_id);
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID > long_id_id
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
        if (totalsize == 0)
        {
            empty_ListTag_31448_11.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_31448_11.DataSource = list.ToList();
        ListTag_31448_11.DataBind();
    }

    public void Load_LabelTag_1699_4()
    {
        SubControl uc_label=(SubControl) LoadControl( "notice.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_1699_4.Controls.Add(uc_label);
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