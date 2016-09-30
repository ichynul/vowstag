<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="Page_test2" EnableViewState="false" %>

<!--2016年09月29日 09:23:13 Powered by VowsTag v-1.4.16.8 http://git.oschina.net/ichynul/vowstag -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    
<asp:Repeater ID="ListTag_44988_1" runat="server">
    <ItemTemplate><!--分页3条-->
    <li><a class="title" href="formTag.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
        <%# Eval("Time") %></span> </li>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_44988_1" runat="server"></asp:Literal>
<asp:Repeater ID="ListTag_44787_5" runat="server">
    <ItemTemplate><!--分页3条-->
    <li><a class="title" href="formTag.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
        <%# Eval("Time") %></span> </li>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_44787_5" runat="server"></asp:Literal>
<form id="form1" runat="server">
    <script id="_tagcall" type="text/javascript" src="/temple/www/page/js/_tagcall.js" ></script>
</form>
</body>
</html>
