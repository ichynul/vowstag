<%@ Control Language="C#" AutoEventWireup="true" CodeFile="artiel_by_categ.ascx.cs" Inherits="Control_artiel_by_categ"  %>

<!-- Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home -->
<div>
    <a class="catename" href="list.aspx?cid=<% = item.ID %>"><% = item.Name %><!--栏目名称-->
    </a>
</div>
<ul class="articlelist">
    
<asp:Repeater ID="ListTag_65_3" runat="server">
    <ItemTemplate>
    <!--list中的list.本标签中引用了外层的item(categ= item.id) -->
    <li><a class="title" href="read.aspx?id=<%# Eval("id") %>"><%# Eval("title") %></a> <span class="time">
        <%# Eval("time") %></span>
        <div class="desc">
            <span><%# SubString(Eval("desc"),80) %></span></div>
    </li>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_65_3" runat="server"></asp:Literal>
</ul>
