<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Page_index" EnableViewState="false" %>

<!--2016年07月27日 17:24:47 Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><% = config.webname %></title>
    <!--引入公共样式、脚本-->
    <link href="/temple/page/css/style.css" rel="stylesheet" type="text/css" />
<script src="/temple/page/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<meta name="keywords" content="<% = config.keyword %>" />

    <!--   static = head   用static标签读取文件head(/static/head.html)中的内容到当前位置-->
</head>
<body>
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_16926_3" runat="server"></asp:PlaceHolder><!--引入label-->
        <!--   label = notice   用label标签加载文件notice(/label/head.html)中的内容到当前位置-->
    </div>
    <div class="navi">
        <ul class="navilist">
    <li><a href="/">首页</a></li>
    <li><a href="listTag.aspx">listTag</a></li>
    <li><a href="readTag.aspx">ReadTag</a></li>
    <li><a href="jsonTag.aspx">jsonTag</a></li>
    <li><a href="formTag.aspx">formTag</a></li>
    <li><a style="color: Red; font-size: 16px;" href="/Make/Maker.aspx">后台管理&gt;&gt;</a></li>
</ul>

        <!--引入static-->
    </div>
    <div class="main">
        <div class="articles">
            <asp:PlaceHolder ID="ListTag_67573_5" runat="server"></asp:PlaceHolder>
<asp:Literal ID="empty_ListTag_67573_5" runat="server"></asp:Literal>

            <!--   list = category ?          用list标签读取表'Category'（表名可忽略大小写）中的数据 ? 后为标签的参数-->
            <!--   orderby = id               按字段ID（可忽略大小写）排序-->
            <!--   desc = true                按降序排序-->
            <!--   take = 99                  取99条数据-->
            <!--   item = artiel_by_categ     样式文件为artiel_by_categ (/item/artiel_by_categ.html) -->
        </div>
    </div>
    <div class="foot">
        <ul class="copyleft">
    <li><a href="index.aspx"><% = config.webname %></a></li>
    <li><% = config.beianhao %></li>
    <li><% = config.copyleft %></li>
    <li><% = config.keyword %></li>
</ul>
<!--引入static-->
    </div>

<form id="form1" runat="server">
    <script id="_tagcall" type="text/javascript" src="/temple/page/js/_tagcall.js" ></script>
</form>
</body>
</html>
