using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Collections.Generic;

using System.Text.RegularExpressions;

public partial class ServerFilePath : System.Web.UI.Page
{
    public string selectedFilePath;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bind();
        }
    }


    private void bind()
    {
        IDictionary<string, string> tmps = new Dictionary<string, string>();
        tmps.Add("pc模板", "~/temple/");
        tmps.Add("手机模板", "~/temple_m/");
        loadTempleFolders(tmps);
        treeFileView_SelectedNodeChanged(null, null);
    }

    private void loadTempleFolders(IDictionary<string, string> tmps)
    {
        foreach (var x in tmps)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = x.Key;
            string temple = Server.MapPath(x.Value);
            if (!Directory.Exists(temple))
            {
                Directory.CreateDirectory(temple);
            }
            getDirectories(temple, tn1);
            tn1.ImageUrl = "../Images/icon/folder.gif";
            treeFileView.Nodes.Add(tn1);
        }
    }

    /// <summary>
    /// 选择节点变化时
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeFileView_SelectedNodeChanged(object sender, EventArgs e)
    {
        selectedFilePath = treeFileView.SelectedValue;
        viewdiv.InnerHtml = string.Format("<iframe id=\"viewpage\" src=\"viewpage.aspx?path={0}\"  scrolling=\"no\" frameborder=\"0\" style=\"height:100%; width:100%; \"></iframe>", selectedFilePath);
    }

    #region Functions

    /// <summary>
    /// 循环遍历获得某一目录下的所有文件信息
    /// </summary>
    /// <param name="path">目录名</param>
    /// <param name="tn">树节点</param>
    private void getDirectories(string path, TreeNode tn)
    {
        string[] fileNames = Directory.GetFiles(path);
        string[] directories = Directory.GetDirectories(path);

        //先遍历这个目录下的文件夹
        foreach (string dir in directories)
        {
            TreeNode subtn = new TreeNode();
            subtn.Value = dir;
            subtn.Text = GetShorterFileName(dir);
            subtn.ImageUrl = "../Images/icon/folder.gif";
            subtn.Expanded = false;
            getDirectories(dir, subtn);
            tn.ChildNodes.Add(subtn);
        }

        //再遍历这个目录下的文件
        foreach (string file in fileNames)
        {
            TreeNode subtn = new TreeNode();
            subtn.ImageUrl = getExtImg(file);
            subtn.Value = file;
            subtn.Text = GetShorterFileName(file);
            //subtn.ShowCheckBox = true;
            tn.ChildNodes.Add(subtn);
        }
    }

    private string getExtImg(string file)
    {
        int i = file.LastIndexOf('.');
        if (i != -1)
        {
            string ext = file.Substring(i + 1);
            if (ext == "aspx" || ext == "cs" || ext == "ascx" || ext == "ashx")
            {
                return "../Images/icon/txt.gif";
            }
            return string.Format("../Images/icon/{0}.gif", ext);
        }
        return "../Images/icon/txt.gif";
    }


    private string GetShorterFileName(string filename)
    {
        return Path.GetFileName(filename);
    }
    #endregion

}
