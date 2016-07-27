<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="Page_test" EnableViewState="false" %>

<!--2016年07月27日 17:24:47 Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    
<asp:Repeater ID="ListTag_24922_1" runat="server">
    <ItemTemplate><!--不分页3条-->
    <li><a class="title" href="formTag.aspx?id=<%# Eval("ID") %>"><%# SubString(Eval("Title"),15) %></a>
        <span class="time"><%# Eval("Time") %></span> </li>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_24922_1" runat="server"></asp:Literal>

    <hr />
    <!--label中的read  读取那条ID为4的文章-->
    <a style="color: Red;" href="read.aspx?id=<% =ReadTag_17580_5.ID %>"><% =ReadTag_17580_5.Title %><% = SubString(ReadTag_17580_5.Title,15) %><!--限制标题最多15个字--></a>
<asp:Repeater ID="ListTag_57983_9" runat="server">
    <ItemTemplate><!--不分页3条-->
    <li><a class="title" href="formTag.aspx?id=<%# Eval("ID") %>"><%# SubString(Eval("Title"),15) %></a>
        <span class="time"><%# Eval("Time") %></span> </li>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_57983_9" runat="server"></asp:Literal>
<form id="form1" runat="server">
    <script id="_tagcall" type="text/javascript" src="/temple/page/js/_tagcall.js" ></script>
</form>
</body>
</html>
