﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年07月11日 14:01:01。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag */

public partial class Control_another : SubControl
{
    protected Article ReadTag_20900_1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Befor_Load_Tags())
            {
                Bind_ReadTag_20900_1();
            }
        }
    }

    public void Bind_ReadTag_20900_1()
    {
        /*id=4*/
        ReadTag_20900_1 = Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID == 4
                    )
                .FirstOrDefault();
        if (ReadTag_20900_1 == null)
        {
            ReadTag_20900_1 = new Article();
        }
    }


    private Entities _Db_Context;
    protected Entities Db_Context
    {
        get
        {
            if (_Db_Context == null)
            {
                _Db_Context = new Entities();
                this.db_created = true;
            }
            return _Db_Context;
        }
    }

    protected override object GetDbObject()
    {
        return this.Db_Context;
    }
}