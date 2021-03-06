﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="datasource.aspx.cs" Inherits="Page_datasource" EnableViewState="false" %>

<!--2016年09月29日 09:30:34 Powered by VowsTag v-1.4.16.8 http://git.oschina.net/ichynul/vowstag -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><% = config.webname %></title>
    <!--引入公共样式、脚本-->
    <link href="/temple/www/page/css/style.css" rel="stylesheet" type="text/css" />
<script src="/temple/www/page/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<meta name="keywords" content="<% = config.keyword %>" />

    <!--测试相对路径的转换-->
    <link href="/temple/www/page/css/kkpager_orange.css" rel="stylesheet" type="text/css" />
    <link href="/temple/www/style.css" rel="stylesheet" type="text/css" />
    <link href="/temple/www/label/style.css" rel="stylesheet" type="text/css" />
    <link href="/temple/m/style.css" rel="stylesheet" type="text/css" />
    <link href="/temple/m/page/style.css" rel="stylesheet" type="text/css" />
    <link href="../../../Make/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/Make/css/control.css" rel="stylesheet" type="text/css" />
    <!--测试相对路径的转换-->
</head>
<body>
    <!-- CMDTag_16209_3 -->
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_1699_4" runat="server"></asp:PlaceHolder><!--引入label-->
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
                        &gt;ListTag
                    </div>
        <div class="articles">
            <p class="about">
                以下list标签均读取article表的数据，且显示的样式一样，这时可以把样式放到一个文件里面，然后用 item=filename 引用，避免重复书写，维护方便</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_39334_7" runat="server">
    <ItemTemplate>
                <li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
                    <%# Eval("Time") %></span> </li>
                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_39334_7" runat="server"></asp:Literal>
 
<asp:Repeater ID="ListTag_31448_11" runat="server">
    <ItemTemplate>
                <li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
                    <%# Eval("Time") %></span> </li>
                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_31448_11" runat="server"></asp:Literal>

            </ul>
            <% = xx(GetCookie("xx","oo")) %> <% = xx(GetCookie("xx")) %> <% = GetCookie("xx") %> <% = GetCookie("xx","oo") %> <% = Request.QueryString["xxoo"] %> <% = Request.QueryString["xxoo"] %>
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
