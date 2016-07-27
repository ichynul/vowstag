<%@ Control Language="C#" AutoEventWireup="true" CodeFile="notice.ascx.cs" Inherits="Control_notice"  %>

<!--读取3条属性istop为true的文章，放在每个页面顶部 -->
<!--因为本文本片段包含list标签，所以必须用 label标签 而不能用static标签-->
<ul class="notice">
    <li>管理猿推荐：</li>
    
<asp:Repeater ID="ListTag_46853_1" runat="server">
    <ItemTemplate>
    <li><a href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a></li>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_46853_1" runat="server"></asp:Literal>

    <li><asp:PlaceHolder ID="LabelTag_16377_4" runat="server"></asp:PlaceHolder><!--label中再包含另一个label--></li>
</ul>
