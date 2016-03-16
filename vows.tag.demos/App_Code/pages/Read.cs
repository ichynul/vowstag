using Tag.Vows.Web;
using model;
using System.Linq;

namespace xx.yy
{
    /// <summary>
    ///read 的摘要说明
    /// </summary>
    public class Read : PageBase
    {
        public Read()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
            this.Befor_Load_Tags = () =>
            {
                string id = Request.QueryString["id"];
                int rid = 0;
                int.TryParse(id, out rid);
                var db = this.GetDbObject() as Entities;
                var article = db.Article.FirstOrDefault(x => x.ID == rid);
                if (article != null)
                {
                    article.View += 1;//点击计数
                    db.SaveChanges();
                }
                return true;
            };
        }

        public override CallbackResult DoCallback()
        {
            string action = this.CallValue("action");
            if (action == "ding")
            {
                string id = this.CallValue("id");
                int did = 0;
                int.TryParse(id, out did);
                var db = this.GetDbObject() as Entities;
                var article = db.Article.FirstOrDefault(x => x.ID == did);
                if (article != null)
                {
                    article.ding += 1;
                    db.SaveChanges();
                    return new CallbackResult(new { msg = "成功的顶了一下~", ding = article.ding });
                }
                else
                {
                    return new CallbackResult(new { msg = "操作失败~", ding = 0 });
                }
            }
            return base.DoCallback();
        }
    }
}