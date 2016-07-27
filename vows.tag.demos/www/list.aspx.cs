﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年07月27日 17:24:47。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag */

public partial class Page_list : xx.yy.PageBase
{
    protected Category ReadTag_31721_6;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ClientScript.GetCallbackEventReference(this, "", "", "");
            if (this.Befor_Load_Tags())
            {
                Bind_ReadTag_31721_6();
                Bind_ListTag_73642_8();
                Load_LabelTag_16703_3();
            }
        }
    }

    public void Bind_ReadTag_31721_6()
    {
        /*id=request.cid*/
        long long_cid = long.MinValue;
        long.TryParse("" + Request.QueryString["cid"],out long_cid);
        ReadTag_31721_6 = Db_Context.Category
                .Where( b=>
                     b.ID == long_cid
                    )
                .FirstOrDefault();
        if (ReadTag_31721_6 == null)
        {
            ReadTag_31721_6 = new Category();
        }
    }

    public void Bind_ListTag_73642_8()
    {
        /*categ=request.cid&orderby=time,istop&desc=true&pagesize=4*/
        int int_cid = int.MinValue;
        int.TryParse("" + Request.QueryString["cid"],out int_cid);
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.Categ == int_cid
                    )
                    select new
                    {
                        a.ID,
                        a.Title,
                        a.Time,
                        a.Content,
                        a.Img,
                        a.IsTop
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.Time)
                    .ThenByDescending(c=>c.IsTop).Skip((page - 1) * 4).Take(4);
        /*分页，每页显示4;*/
        string urlparams = RemovePageParams(Request.RawUrl);
        if (totalsize > 4)
        {
            Pager pager= new Pager(totalsize, page, 4,urlparams, 3, "上一页", "下一页", "...", true);
            PagerTag_17972_15.Text = "<div id='pager' class='mypager'>" + pager.MakeLinks() +"</div>"; 
        }
        PagerTag_17972_15.Text += "\r\n<div id='pagerinfo' style='display:none;'>"
                + "\r\n<input type='hidden' id='list_size' value='" + totalsize + "' data-info='总纪录条数' />"
                + "\r\n<input type='hidden' id='num_per_page' value='" + 4 + "' data-info='每页显示' />"
                + "\r\n<input type='hidden' id='current_page' value='" + page + "' data-info='当前页' />"
                 + "\r\n<input type='hidden' id='current_url_params' value='" + urlparams + "' data-info='当前url参数（不带page参数）' />"
                + "\r\n</div>";
        if (totalsize == 0)
        {
            empty_ListTag_73642_8.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_73642_8.DataSource = list.ToList();
        ListTag_73642_8.DataBind();
    }

    public void Load_LabelTag_16703_3()
    {
        SubControl uc_label=(SubControl) LoadControl( "notice.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_16703_3.Controls.Add(uc_label);
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