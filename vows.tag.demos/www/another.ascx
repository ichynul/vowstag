<%@ Control Language="C#" AutoEventWireup="true" CodeFile="another.ascx.cs" Inherits="Control_another"  %>

<!--label中的read  读取那条ID为4的文章-->
<a style="color:Red;" href="read.aspx?id=<% =ReadTag_20175_1.ID %>"><% = SubString(ReadTag_20175_1.Title,15) %><!--限制标题最多15个字--></a>
