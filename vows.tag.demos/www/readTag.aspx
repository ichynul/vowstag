<%@ Page Language="C#" AutoEventWireup="true" CodeFile="readTag.aspx.cs" Inherits="Page_readTag" EnableViewState="false" %>

<!--2016年07月27日 17:24:47 Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><% =read.Title %>--<% = config.webname %></title>
    <!--引入公共样式、脚本-->
    <link href="/temple/page/css/style.css" rel="stylesheet" type="text/css" />
<script src="/temple/page/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<meta name="keywords" content="<% = config.keyword %>" />

</head>
<body>
    <!-- ReadTag_22990_4 --><!--单标签形式的read,可以在页面任何位置读取其read字段值；每个页面最多只能有一个全局read-->
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_16646_5" runat="server"></asp:PlaceHolder><!--引入label-->
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
                        &gt;readTag
        </div>
        <div class="read">
            <div class="infos">
                <h3>
                    <% =read.Title %><!-- 也可写成<% =read.Author %>-->
                </h3>
                <span>作者：<% =read.Author %></span><span>日期：<% = TimeFormat(read.Time,"MM月dd日") %><!-- 格式化日期 -->
                </span><span>点击：<% =read.View %></span>
            </div>
            <img src="<% =read.Img %>" style="width: 250px; margin-left: 250px;" />
            <div class="content">
                <% = SubString(read.Content,500) %>
                <!-- 使用后端方法，截取指定长度-->
            </div>
            <div class="links">
                <span>上一篇： </span>
                <!--用list标签读取上一篇-->
                
<asp:Repeater ID="ListTag_68317_15" runat="server">
    <ItemTemplate><!--此处用字段id与read参数比较 -->
                                <a><%# Eval("Title") %></a>
                                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_68317_15" runat="server"></asp:Literal>

                                <span>下一篇：</span>
                <!--用read标签读取下一篇-->
                <!--全局read的字段可作为局部read的参数 -->
                <!--此处用字段id与read.id(read标签的一个字段)比较-->
                                <a><% =ReadTag_48239_17.Title %></a>
                                <!--必须有结尾，一个页面中只能有一个全局read -->
                            </div>
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
