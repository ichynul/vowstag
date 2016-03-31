using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using model;
using Tag.Vows.Tool;
using Tag.Vows.Bean;
using System.Collections.Generic;

public partial class Admin_Temple_Maker : Page
{
    private DirectoryInfo di;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            findPages();
            tagTypeCHange(null, null);
        }
    }

    private void findPages()
    {
        string path = Server.MapPath(mod_type.SelectedIndex == 0 ? "~/temple/" : "~/temple_m/");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        checkDirs(path);
        pageinfo.Text = "";
        makeresult.Text = "";
        path += "page\\";
        di = new DirectoryInfo(path);
        pages.DataSource = di.GetFiles().ToList()
            .Where(a => a.Extension == ".htm" || a.Extension == ".html")
            .Select(f => new
            {
                f.Name,
                f.FullName,
                f.Extension,
                Length = f.Length / 1024.00,
                f.LastWriteTime,
                _path = this.input_path.Text + "page/",
                www = (mod_type.SelectedIndex == 0 ? "/www/" : "/m/") + Regex.Replace(f.Name, @"\.(?:html|htm)", "") + ".aspx",
            });
        pages.DataBind();
    }

    private void checkDirs(string path)
    {
        string[] dirs = new string[] { "item", "label", "page", "static" };
        string newpath = "";
        foreach (string s in dirs)
        {
            newpath = string.Format("{0}{1}\\", path, s);
            if (!Directory.Exists(newpath))
            {
                Directory.CreateDirectory(newpath);
            }
        }
    }

    protected string toPageNaem(object name)
    {
        if (name == null)
        {
            return "";
        }
        string str = name.ToString();
        if (str.IndexOf('.') != -1)
        {
            return str.Split('.')[0];
        }
        return "";
    }

    private string getHtml(string html)
    {
        return html;
    }

    private string getTable(string tag, string table)
    {
        if (string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(table))
        {
            return "";
        }
        if (table.ToLower() == "article")
        {//给Article表设置一个限定，无论查询中是否有 islok!=true 条件，我们都加上这个条件（锁定的文章我们永远不希望在前台展示）
            //
            return "Db_Context.Article.Where(x => x.IsLock != true)";//此处应写标准的linq语句，表名及字段区分大小写
        }
        return "Db_Context." + table;
    }

    protected void doMake(object sender, EventArgs e)
    {
        makeresult.Text = "";
        string path = HttpContext.Current.Server.MapPath("~" + this.input_path.Text);
        di = new DirectoryInfo(path);
        if (!di.Exists)
        {
            makeresult.Text = "<span style='color:red;'>模板文件夹路径错误!" + path + "</span>";
        }
        TagConfig __config = new TagConfig();
        __config.GetingHtml += new GetHtml(getHtml);
        __config.GetingTableStr += new GetTableStr(getTable);
        __config.current_pairs = getCurrentTagPair(currenttagpair);
        __config.input = mod_type.SelectedIndex == 0 ? "~/temple/" : "~/temple_m/";
        __config.output = mod_type.SelectedIndex == 0 ? "~/www/" : "~/m/";
        __config.db = new Entities();
        __config.protected_tables = "Admin";//受保护的表，不允许通过标签在前台展示
        __config.creatScriptForAllPages = true;
        __config.DefaultBase = "xx.yy.PageBase";//页面默认继承类
        __config.convert = isconvert.Checked;
        AddDialect(__config);
        if (__config.convert)
        {
            __config.convert_pairs = getCurrentTagPair(toTagpairs);
        }
        else if (clearBefor.Checked)
        {
            clearOutpu(__config.input, __config.output);
            clearBefor.Checked = false;
        }
        var its = pages.Items;
        CheckBox cb = null;
        foreach (RepeaterItem it in its)
        {
            cb = it.FindControl("cb_id") as CheckBox;
            if (cb.Checked)
            {
                if (showTagInfos.Checked)
                {
                    string text = "";
                    makeresult.Text += __config.MakePage(cb.Text.Trim(), out text);
                    makeresult.Text += text;
                }
                else
                {
                    makeresult.Text += __config.MakePage(cb.Text.Trim());
                }
            }
        }
        makeresult.Text += "<span style='color:green;'>" +
            (isconvert.Checked ? @"转换标签完成！转换后的文件保存在各目录的下的'\convert\'里." : "生成页面完成！")
            + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + DateTime.Now.ToString() + "!</span>";
    }

    private void AddDialect(TagConfig config)
    {
        TableDialect article = new TableDialect("Article", new HashSet<string>()
        {
            "文章"
        });

        article.Fields.Add(new Dialect("Id", new HashSet<string>() { "主键ID" }));
        article.Fields.Add(new Dialect("Categ", new HashSet<string>() { "栏目", "分类" }));
        article.Fields.Add(new Dialect("UID", new HashSet<string>() { "用户ID", "作者ID" }));
        article.Fields.Add(new Dialect("title", new HashSet<string>() { "文章标题", "标题" }));
        article.Fields.Add(new Dialect("Desc", new HashSet<string>() { "文章摘要", "摘要" }));
        article.Fields.Add(new Dialect("Author", new HashSet<string>() { "作者", "作者名字" }));
        article.Fields.Add(new Dialect("Img", new HashSet<string>() { "图片", "logo" }));
        article.Fields.Add(new Dialect("Content", new HashSet<string>() { "文章内容", "正文" }));
        article.Fields.Add(new Dialect("View", new HashSet<string>() { "查看", "点击" }));
        article.Fields.Add(new Dialect("IsLock", new HashSet<string>() { "锁定", "禁止查看" }));
        article.Fields.Add(new Dialect("IsTop", new HashSet<string>() { "置顶", "是否置顶" }));
        article.Fields.Add(new Dialect("ding", new HashSet<string>() { "被顶", "被顶次数" }));
        article.Fields.Add(new Dialect("time", new HashSet<string>() { "发布时间", "时间" }));

        config.AddDialect(article);

        TableDialect categgory = new TableDialect("Category", new HashSet<string>()
        {
            "栏目"
        });

        categgory.Fields.Add(new Dialect("Id", new HashSet<string>() { "主键ID" }));
        categgory.Fields.Add(new Dialect("name", new HashSet<string>() { "栏目名称", "名称" }));
        /*
         * 
         * .......
         * 
         */
        config.AddDialect(categgory);

    }

    private void clearOutpu(string tempPath, string output)
    {
        di = new DirectoryInfo(output);
        if (!di.Exists)
        {
            return;
        }

        var pages = di.GetFiles().Where(a => a.Extension == ".aspx" || a.Extension == ".ascx" || a.Extension == ".cs" || a.Extension == ".ashx");
        foreach (var p in pages)
        {
            try
            {
                p.Delete();
            }
            catch (Exception e)
            {
                makeresult.Text += e.ToString() + "<br />";
            }
        }
    }


    protected void typechanged(object sender, EventArgs e)
    {
        output_path.Text = mod_type.SelectedIndex == 0 ? "/www/" : "/m/";
        input_path.Text = mod_type.SelectedIndex == 0 ? "/temple/" : "/temple_m/";
        toview.NavigateUrl = mod_type.SelectedIndex == 0 ? "/www/" : "/m/";
        findPages();
    }

    protected void tagTypeCHange(object sender, EventArgs e)
    {
        tagInfo.Text = "";
        string[] tagpair = getCurrentTagPair(currenttagpair);
        tagInfo.Text = string.Format("单标签演示:<br />&nbsp;&nbsp;&nbsp;&nbsp;{0} user.naem {1} ;<br />双标签演示:"
                + "<br />&nbsp;&nbsp;&nbsp;&nbsp;{0} list = newss ? id > 8 & orderby = time {1} "
                + "<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;文章标题：{0} itemt.itle {1} "
                + "<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;发布时间：{0} itemt.time {1} " +
                "<br />&nbsp;&nbsp;&nbsp;&nbsp;{0}list{1}", tagpair[0], tagpair[1]);
        if (tagpair[0] == "{")
        {
            tagInfo.Text += "<br />不推荐，某些情况下会把js代码误判为标签，且易与一些js模板库的标签混淆";
        }
    }
    protected void isConvert(object sender, EventArgs e)
    {
        doConvert.Visible = isconvert.Checked;
        make.Text = isconvert.Checked ? "转换选中" : "生成选中";
    }

    protected void rebind(object sender, EventArgs e)
    {
        findPages();
        chall.Checked = false;
    }

    private string[] getCurrentTagPair(ListControl list)
    {
        string tagLeft = "";
        string tagRight = "";
        switch (list.SelectedIndex)
        {

            case 0:
                tagLeft = "{@";
                tagRight = "}";
                break;
            case 1:
                tagLeft = "{#";
                tagRight = "}";
                break;
            case 2:
                tagLeft = "{$";
                tagRight = "}";
                break;
            case 3:
                tagLeft = "{%";
                tagRight = "}";
                break;
            case 4:
                tagLeft = "{&";
                tagRight = "}";
                break;
            case 5:
                tagLeft = "{";
                tagRight = "}";
                break;
        }
        return new string[] { tagLeft, tagRight };
    }
}