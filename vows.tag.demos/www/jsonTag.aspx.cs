﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;
using Newtonsoft.Json;
using System.Dynamic;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年07月28日 15:48:54。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag v-1.4.16.727 http://git.oschina.net/ichynul/vowstag */

public partial class Page_jsonTag : xx.yy.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ClientScript.GetCallbackEventReference(this, "", "", "");
            if (this.Befor_Load_Tags())
            {
                Bind_ListTag_4590_6();
                Load_LabelTag_16152_3();
            }
        }
    }

    public void Bind_ListTag_4590_6()
    {
        /*orderby=id&desc=true&take=99*/
        var list = from a in Db_Context.Category
                    select new
                    {
                        a.ID,
                        a.Name
                    };
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.ID).Take(99);
        /*不分页，显示前 99条数据;*/
        if (totalsize == 0)
        {
            empty_ListTag_4590_6.Text = "<div class='emptydiv'><span class='emptytext'>暂无内容</span></div>";
            return;
        }
        ListTag_4590_6.DataSource = list.ToList();
        ListTag_4590_6.DataBind();
    }

    public void Load_LabelTag_16152_3()
    {
        SubControl uc_label=(SubControl) LoadControl( "notice.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_16152_3.Controls.Add(uc_label);
    }

    public CallbackResult CallBack_JsonTag_71689_11()
    {
        /*orderby=时间&pagesize=3&fields=title,时间,time,desc,img,id*/
        string jsonname = this.CallValue("jsonname");
        if (jsonname != "JsonTag_71689_11")
        {
            return null;
        }
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                    select new
                    {
                        a.Title,
                        a.Time,
                        a.Desc,
                        a.Img,
                        a.ID
                    };
        int __page = 0;
        int.TryParse(this.CallValue("page"), out __page);
        __page = __page < 1 ? 1 : __page;
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.Time).Skip((__page - 1) * 3).Take(3);
        dynamic json = new ExpandoObject();
        json.jsonname = "JsonTag_71689_11";
        json.tagstr = "orderby=时间&pagesize=3&fields=title,时间,time,desc,img,id";
        json.skip = (__page - 1) * 3;
        json.pagesize = 3;
        json.listsize = totalsize;
        json.page = __page;
        json.over = list.Count() < 3;
        json.data = list;
        CallbackResult call = new CallbackResult(json) ;
        call.type = "jsoncall";
        return call;
    }

    public CallbackResult CallBack_JsonTag_85707_12()
    {
        /*orderby=time&pagesize=3&fields=title,time,desc,img,id&categ=call.cid*/
        string jsonname = this.CallValue("jsonname");
        if (jsonname != "JsonTag_85707_12")
        {
            return null;
        }
        int int_cid = int.MinValue;
        int.TryParse("" + CallString["cid"],out int_cid);
        var list = from a in Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.Categ == int_cid
                    )
                    select new
                    {
                        a.Title,
                        a.Time,
                        a.Desc,
                        a.Img,
                        a.ID
                    };
        int __page = 0;
        int.TryParse(this.CallValue("page"), out __page);
        __page = __page < 1 ? 1 : __page;
        int totalsize = list.Count();
        list = list.OrderByDescending(c=>c.Time).Skip((__page - 1) * 3).Take(3);
        dynamic json = new ExpandoObject();
        json.jsonname = "JsonTag_85707_12";
        json.tagstr = "orderby=time&pagesize=3&fields=title,time,desc,img,id&categ=call.cid";
        json.skip = (__page - 1) * 3;
        json.pagesize = 3;
        json.listsize = totalsize;
        json.page = __page;
        json.over = list.Count() < 3;
        json.data = list;
        CallbackResult call = new CallbackResult(json) ;
        call.type = "jsoncall";
        return call;
    }

    public override CallbackResult TagCallback()
    {
        CallbackResult callback = null;
        callback = CallBack_JsonTag_71689_11();
        if (callback != null)
        {
            return callback;
        }
        callback = CallBack_JsonTag_85707_12();
        if (callback != null)
        {
            return callback;
        }
        return callback;
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