<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="Page_test2" EnableViewState="false" %>

<!--2016年10月09日 14:26:35 Powered by VowsTag v-1.4.16.8 http://git.oschina.net/ichynul/vowstag -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    {#list=article? "3"=url.type &id=4&pagesize=3

    
<asp:Repeater ID="ListTag_44909_2" runat="server">
    <ItemTemplate><!--分页3条-->
    <span>
                <%# Eval("Title") %> </span>
        </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_44909_2" runat="server"></asp:Literal>
<form id="form1" runat="server">
    <script id="_tagcall" type="text/javascript" src="/temple/www/page/js/_tagcall.js" ></script>
</form>
</body>
</html>
