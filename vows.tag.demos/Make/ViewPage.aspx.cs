using model;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Tag.Vows.Tool;

public partial class WebMana_Temple_ViewPage : System.Web.UI.Page
{
    string selectedFilePath;
    protected void Page_Load(object sender, EventArgs e)
    {
        selectedFilePath = Request.QueryString["path"];
        if (!IsPostBack)
        {
            show();
        }
    }

    private void show()
    {
        tagArea.Visible = false;
        if (string.IsNullOrEmpty(selectedFilePath))
        {
            Labelname.Text = "请选择文件进行操作！";
            btnSave.Visible = false;
            editable.Value = "no";
            return;
        }
        Labelname.Text = GetShorterFileName(selectedFilePath);
        if (Regex.IsMatch(selectedFilePath, @".*?\.(?:html|htm|xhtml|xml|json|js|css|text)$", RegexOptions.IgnoreCase))
        {
            Content.Text = ReadFile(selectedFilePath);
            editable.Value = "yes";
            btnSave.Visible = true;
            Content.Visible = true;
            imgview.Visible = false;
            tagArea.Visible = true;
            TagConfig __config = new TagConfig();
            __config.current_pairs = getCurrentTagPair(currenttagpair);
            __config.db = new Entities();
            __config.protected_tables = "Manage|ManageRole|WxConfig|Log";
            __config.creatScriptForAllPages = true;
            __config.DefaultBase = "web.x2015x.UserBase";
            var taglist = __config.GetTagList(selectedFilePath);
            StringBuilder sb = new StringBuilder("<ul>");

            foreach (var x in taglist)
            {
                sb.AppendFormat("<li data-no='{0}'><span title='{2}'>{1}</span></li>", x.no_, x.text.Replace(">", "&gt;").Replace("<", "&lt;"),
                      ("" + x.style).Replace(">", "&gt;").Replace("<", "&lt;"));
            }
            sb.Append("</ul>");
            tagList.Text = sb.ToString();
        }
        else if (Regex.IsMatch(selectedFilePath, @".*?\.(?:aspx|ascx|cs)$", RegexOptions.IgnoreCase))
        {
            Content.Text = ReadFile(selectedFilePath);
            editable.Value = "yes";
            btnSave.Visible = true;
            Content.Visible = true;
            imgview.Visible = false;
            btnSave.Visible = false;
        }
        else if (Regex.IsMatch(selectedFilePath, @".*?\.(?:jpeg|jpg|png|gif}bmp)$", RegexOptions.IgnoreCase))
        {
            Content.Visible = false;
            imgview.Visible = true;
            editable.Value = "no";
            imgview.Src = getImagePath(selectedFilePath);
            btnSave.Visible = false;
        }
        else
        {
            Content.Visible = true;
            Content.Text = "当前文件夹或文件不支持编辑";
            editable.Value = "no";
            btnSave.Visible = false;
            imgview.Visible = false;
        }
    }

    protected void tagTypeCHange(object sender, EventArgs e)
    {
        show();
    }

    private string getImagePath(string path)
    {
        Match m = Regex.Match(path, string.Format(@"^{0}(?<url>.*?)$", Server.MapPath("~/")).Replace("\\", "\\\\"), RegexOptions.IgnoreCase);
        if (m.Success)
        {
            GroupCollection gc = m.Groups;
            return "/" + gc["url"].Value.Replace("\\", "/");
        }
        return "";
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        int i = selectedFilePath.LastIndexOf('.');
        if (i != -1)
        {
            string ext = selectedFilePath.Substring(i + 1);
            if (ext == "aspx" || ext == "cs" || ext == "ascx" || ext == "ashx")
            {
                // JscriptPrint("操作失败！", "", "");

                Response.Write("操作失败");
                return;
            }
        }

        string content = this.Content.Text;
        WriteFile(selectedFilePath, content);
        //JscriptPrint("模板修改成功啦！", "", "success");
        Response.Write("模板修改成功啦");

    }

    public void WriteFile(string FileName, string content)
    {
        byte[] data = Encoding.UTF8.GetBytes(content);
        FileStream file = null;
        try
        {
            file = new FileStream(FileName, FileMode.Create);
            file.Write(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }
    public string ReadFile(string FileName)
    {
        string ret = "";
        FileStream files = null;
        try
        {
            files = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(files);
            ret = sr.ReadToEnd();
            sr.Close();
        }
        catch (Exception ex)
        {
            // throw ex;
        }
        finally
        {
            if (files != null)
            {
                files.Close();
            }
        }
        return ret;
    }

    private string GetShorterFileName(string filename)
    {
        return Path.GetFileName(filename);
    }

    private string[] getCurrentTagPair(ListControl list)
    {
        string tagLeft = "";
        string tagRight = "";

        switch (list.SelectedIndex)
        {
            case 0:
                tagLeft = "{";
                tagRight = "}";
                break;
            case 1:
                tagLeft = "{@";
                tagRight = "}";
                break;
            case 2:
                tagLeft = "{#";
                tagRight = "}";
                break;
            case 3:
                tagLeft = "{$";
                tagRight = "}";
                break;
            case 4:
                tagLeft = "{%";
                tagRight = "}";
                break;
        }
        return new string[] { tagLeft, tagRight };
    }
}