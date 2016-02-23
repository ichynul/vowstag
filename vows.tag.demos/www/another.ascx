<%@ Control Language="C#" AutoEventWireup="true" CodeFile="another.ascx.cs" Inherits="Control_another"  %>

<!-- Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home -->
<!--label中的read  读取那条ID为4的文章-->
<a style="color:Red;" href="read.aspx?id=<% =ReadTag_20_1.ID %>"><% = SubString(ReadTag_20_1.Title,15) %><!--限制标题最多15个字--></a>
