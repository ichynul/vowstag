﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;
using Newtonsoft.Json;
using System.Dynamic;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年07月25日 16:08:22。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag */

public partial class Page_formTag : xx.yy.PageBase
{
    protected Article read;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ClientScript.GetCallbackEventReference(this, "", "", "");
            if (this.Befor_Load_Tags())
            {
                Bind_ReadTag_30714_8();
                Load_LabelTag_16347_5();
            }
        }
    }

    public void Bind_ReadTag_30714_8()
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

    public void Load_LabelTag_16347_5()
    {
        SubControl uc_label=(SubControl) LoadControl( "notice.ascx");
        uc_label.SetDb(this.Db_Context);
        uc_label.SetConfig(this.config);
        LabelTag_16347_5.Controls.Add(uc_label);
    }

    public CallbackResult CallBack_FormTag_65671_9()
    {
        /*id=request.id&allowempty=author,view&action=edit*/
        string formname = this.CallValue("formname");
        if (formname != "FormTag_65671_9")
        {
            return null;
        }
        dynamic error = new ExpandoObject();
        error.formname = formname;
        CallbackResult call = new CallbackResult(error) ;
        call.type = "formcall";
        Article _article = null ;
        long long_id = long.MinValue;
        long.TryParse("" + Request.QueryString["id"],out long_id);

        _article = Db_Context.Article.Where(x => x.IsLock != true).FirstOrDefault( b=>
                     b.ID == long_id
                    );
        if (_article == null)
        {
            /*编辑模式（action = edit），未找到记录则返回*/;
            error.code = 3;
            error.msg = "操作失败：not found！";
            error.dom = "";
            return call;
        }
        string _Title = this.CallValue("Title");
        if (string.IsNullOrEmpty(_Title))
        {
            error.code = 1;
            error.msg = "不能为空！";
            error.dom = "Title";
            return call;
        }
        _article.Title = _Title;
        string _Author = this.CallValue("Author");
        _article.Author = _Author;
        string _Time = this.CallValue("Time");
        DateTime __Time = DateTime.Now;
        if (DateTime.TryParse(_Time,out __Time))
        {
            _article.Time = __Time ;
        }
        else
        {
            if (string.IsNullOrEmpty(_Time))
            {
                error.code = 1;
                error.msg = "不能为空！";
                error.dom = "Time";
                return call;
            }
            else
            {
                error.code = 2;
                error.msg = "请输入正确的时间";
                error.dom = "Time";
                return call;
            }
        }
        string _View = this.CallValue("View");
        int __View = 0 ;
        if (int.TryParse(_View,out __View))
        {
            _article.View = __View ;
        }
        string _Content = this.CallValue("Content");
        if (string.IsNullOrEmpty(_Content))
        {
            error.code = 1;
            error.msg = "不能为空！";
            error.dom = "Content";
            return call;
        }
        _article.Content = _Content;
        //
        Db_Context.SaveChanges();
        error.code = 0;
        error.msg = "操作成功！";
        error.dom = "";
        return call;
    }

    public override CallbackResult TagCallback()
    {
        CallbackResult callback = null;
        callback = CallBack_FormTag_65671_9();
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