﻿using System;
using System.Linq;
using Tag.Vows.Web;
using model;

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//    生成时间: 2016年07月28日 15:48:54。
//    应避免手动更改此文件。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
/*  Powered by VowsTag v-1.4.16.727 http://git.oschina.net/ichynul/vowstag */

public partial class Control_another : SubControl
{
    protected Article ReadTag_20887_1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Befor_Load_Tags())
            {
                Bind_ReadTag_20887_1();
            }
        }
    }

    public void Bind_ReadTag_20887_1()
    {
        /*id=4*/
        ReadTag_20887_1 = Db_Context.Article.Where(x => x.IsLock != true)
                .Where( b=>
                     b.ID == 4
                    )
                .FirstOrDefault();
        if (ReadTag_20887_1 == null)
        {
            ReadTag_20887_1 = new Article();
        }
    }


    protected Entities Db_Context;
    public override void SetDb(object _dbcontext){
        this.Db_Context = _dbcontext as Entities;
    }

    protected override object GetDbObject()
    {
        return this.Db_Context;
    }
}