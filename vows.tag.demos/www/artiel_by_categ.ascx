<%@ Control Language="C#" AutoEventWireup="true" CodeFile="artiel_by_categ.ascx.cs" Inherits="Control_artiel_by_categ"  %>

<div>
    <a class="catename" href="list.aspx?cid=<% = item.ID %>"><% = SubString(item.Name,20) %><!--栏目名称-->
    </a>
</div>
<ul class="articlelist">
    
<asp:Repeater ID="ListTag_65732_3" runat="server">
    <ItemTemplate>
    <!--list中的list.本标签中引用了外层的item(categ= item.id) -->
    <li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
        <%# Eval("Time") %></span>
        <div class="desc">
            <span><%# SubString(Eval("Desc"),80) %></span></div>
    </li>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_65732_3" runat="server"></asp:Literal>

</ul>
