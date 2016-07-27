<%@ Page Language="C#" AutoEventWireup="true" CodeFile="read.aspx.cs" Inherits="Page_read" ValidateRequest="false" EnableViewState="false" %>

<!--2016年07月25日 16:08:23 Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag -->
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
        <!-- CMDTag_37129_4 -->
    <!--指定页面处理类 -->
        <!-- ReadTag_30465_5 --><!--单标签形式的read,可以在页面任何位置读取其read字段值；每个页面最多只能有一个全局read-->
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_16139_6" runat="server"></asp:PlaceHolder><!--引入label-->
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
                        &gt;<a href="list.aspx?cid=<% =ReadTag_30475_9.ID %>&page=<% = page %>"><% =ReadTag_30475_9.Name %></a><!--局部的read-->
                        &gt;<a><% =read.Title %></a>
        </div>
        <div class="read">
            <div class="infos">
                <h3>
                    <% =read.Title %>
                </h3>
                <span>作者：<% =read.Author %></span><span>日期：<% = TimeFormat(read.Time) %></span><span>点击：<% =read.View %></span>
            </div>
            <img src="<% =read.Img %>" style="width: 250px; margin-left: 250px;" />
            <div class="content">
                <% =read.Content %>
            </div>
            <div class="links">
                <span>上一篇： </span>
                <!--用list标签读取上一篇-->
                
<asp:Repeater ID="ListTag_71545_20" runat="server">
    <ItemTemplate><!--此处用字段id与url参数比较 -->
                                <a href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a>
                                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_71545_20" runat="server"></asp:Literal>

                                <span>下一篇：</span>
                <!--用list标签读取下一篇-->
                
<asp:Repeater ID="ListTag_55410_23" runat="server">
    <ItemTemplate>
                <!--此处用字段id与read.id(read标签的一个字段)比较,这里跟request.id差不多-->
                                <a href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a>
                                
                                </ItemTemplate>
</asp:Repeater>
            <% if (ListTag_55410_23Empty) %>
<% { %>
<!--empty标签对，作用与emptytext差不多，但emptytext仅支持简单的文字，而empty可包含复杂的文本（如html） -->
                                <span style="color: Red;">没有了~</span> <% = Request.QueryString["id"] %>
                                
<% } %>

                            </div>
            <div class="ding">
                <a id="dingyixia" style="color: Red; font-size: 15px;" href="javascript:;" onclick='ding()'>
                    顶一个(<% =read.ding %>)</a>
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
    <script type="text/javascript">
        function ding() {
            $('#dingyixia').text('操作中...');
            _tagcall.$('action=ding&id=<% = Request.QueryString["id"] %>');
        }

        function CallbackSuccess(resultstr, context) {
            var obj = eval('(' + resultstr + ')');
            if (obj.type) {
                if (obj.type == "mycall") {//自定义请求的返回
                    $('#dingyixia').text('顶一个(' + obj.result.ding + ')');
                    alert(obj.result.msg);
                }
                else if (obj.type == "formcall") {//form标签请求的返回

                }
                else if (obj.type == "jsoncall") {//json标签请求的返回

                }
                else {
                    alert(resultstr);
                }
            }
            else {
                alert(resultstr);
            }
        }
    </script>
    

<form id="form1" runat="server">
    <script id="_tagcall" type="text/javascript" src="/temple/page/js/_tagcall.js" ></script>
</form>
</body>
</html>
