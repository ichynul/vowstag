<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ifTag.aspx.cs" Inherits="Page_ifTag" EnableViewState="false" %>

<!--2016年07月20日 13:58:16 Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
 <input type="hidden" id="parentid" value="<% = Request.QueryString["p"] %>" />
    <% if (Request.QueryString["kwd"] =="22") %>
<% { %> 
        <ul class="navilist">
    <li><a href="/">首页</a></li>
    <li><a href="listTag.aspx">listTag</a></li>
    <li><a href="readTag.aspx">ReadTag</a></li>
    <li><a href="jsonTag.aspx">jsonTag</a></li>
    <li><a href="formTag.aspx">formTag</a></li>
    <li><a style="color: Red; font-size: 16px;" href="/Make/Maker.aspx">后台管理&gt;&gt;</a></li>
</ul>
 
        <% if (Request.QueryString["kwd"] =="2") %>
<% { %>
            <asp:PlaceHolder ID="LabelTag_15924_3" runat="server"></asp:PlaceHolder>
            <% if (Request.QueryString["xx"]=="oo") %>
<% { %>
                
<asp:Repeater ID="ListTag_20209_4" runat="server">
    <ItemTemplate>
                    <%# Eval("Title") %>
                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_20209_4" runat="server"></asp:Literal>

            <% } %>

        <% } %>

    <% } %>
<asp:Repeater ID="ListTag_2628_6" runat="server">
    <ItemTemplate><!--分页3条-->
        <li><a class="title" href="formTag.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
            <%# Eval("Time") %></span> </li>
            
     </ItemTemplate>
</asp:Repeater>
            <% if (ListTag_2628_6Empty) %>
<% { %>

                xxoo
            
<% } %>
<asp:Repeater ID="ListTag_3754_10" runat="server">
    <ItemTemplate><!--分页3条-->
        <li><a class="title" href="formTag.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
            <%# Eval("Time") %></span> </li>
            
     </ItemTemplate>
</asp:Repeater>
            <% if (ListTag_3754_10Empty) %>
<% { %>

                xxoo
            
<% } %>
<form id="form1" runat="server">
    <script id="_tagcall" type="text/javascript" src="/temple/page/js/_tagcall.js" ></script>
</form>
</body>
</html>
