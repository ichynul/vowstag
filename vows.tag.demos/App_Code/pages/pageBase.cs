using Tag.Vows.Web;
using model;
using System.Web;
using model;

namespace xx.yy
{

    /// <summary>
    ///pageBase 的摘要说明
    /// </summary>
    public class PageBase : TagPage
    {
        protected User user;

        public PageBase()
        {
            this.config.webname = "vowstag demos";
            this.config.beianhao = "滇ICP-1234555";
            this.config.copyleft = "Copyleft © ichynul 2016";
            this.config.keyword = "Vowstag通用标签";
            this.Befor_Load_Tags += () =>
            {
                login();
                return true;
            };
        }

        protected void login()
        {
            user = new User();
            user.ID = 1;
            user.Name = "小明";
            user.Gender = "男";
            //
            Session["uid="] = 1;
            Session["uname="] = "小明";
            //
            WriteCookie("uid", "1");
            WriteCookie("uname", "小明");
        }

        public void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            Response.AppendCookie(cookie);
        }

        private Entities ___db;
        protected Entities __db
        {
            get
            {
                if (___db == null)
                {
                    object d = this.GetDbObject();
                    if (d != null)// 某些情况下，此时调用this.GetDbObject() 为null（直接 new UserBase(); ）
                    {
                        ___db = d as Entities;
                    }
                    else
                    {
                        ___db = new Entities();
                    }
                }
                return ___db;
            }
        }
    }



}