<%@ Page Language="C#" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="Page_list" EnableViewState="false" %>

<!--2016年07月28日 15:48:54 Powered by VowsTag v-1.4.16.727 http://git.oschina.net/ichynul/vowstag -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><% = config.webname %></title>
    <!--引入公共样式、脚本-->
    <link href="/temple/www/page/css/style.css" rel="stylesheet" type="text/css" />
<script src="/temple/www/page/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<meta name="keywords" content="<% = config.keyword %>" />

</head>
<body>
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_16851_3" runat="server"></asp:PlaceHolder><!--引入label-->
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

    </div>
    <div class="main">
        <div class="location">
            当前位置：<a href="index.aspx"><% = config.webname %></a>
                        &gt;<a><% =ReadTag_31958_6.Name %></a>
                    </div>
        <div class="articles">
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_7367_8" runat="server">
    <ItemTemplate><!--每页显示4条->
                <!--list中的list.本标签中引用了url参数(categ= request.cid) -->
                <li><a class="title" href="read.aspx?id=<%# Eval("ID") %>&page=<% = page %>"><%# Eval("Title") %></a> <span class="time">
                    <%# Eval("Time") %></span>
                    <div class="desc">
                        <span><%# SubString(Eval("Content"),200) %></span></div>
                    <img src="<%# Eval("Img") %>" style="width: 100px; height: 100px; float: right" alt='' />
                </li>
                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_7367_8" runat="server"></asp:Literal>

            </ul>
            <asp:Literal ID="PagerTag_17865_15" runat="server"></asp:Literal><!--分页标签 type=cs为cs代码直接输出分页html,type=js则会输出分页需要的参数-->
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
    <script id="_tagcall" type="text/javascript" src="/temple/www/page/js/_tagcall.js" ></script>
</form>
</body>
</html>
