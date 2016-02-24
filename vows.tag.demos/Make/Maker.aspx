<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Maker.aspx.cs" Inherits="Admin_Temple_Maker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>页面管理</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/function.js"></script>
    <script type="text/javascript">
        $(function () {

            $("#chall").click(function () {
                var ck = $(this).hasClass("checked");
                $(".checkall input[type='checkbox']").each(function () {
                    if (ck) {
                        if ($(this).hasClass("checked")) {
                            this.click();
                        }
                    }
                    else {
                        if (!$(this).hasClass("checked")) {
                            this.click();
                        }
                    }
                });
                if ($(this).hasClass("checked")) {
                    $(this).removeClass("checked");
                }
                else {
                    $(this).addClass("checked");
                }
            });
            $(".checkall input[type='checkbox']").click(function () {
                if ($(this).hasClass("checked")) {
                    $(this).removeClass("checked");
                }
                else {
                    $(this).addClass("checked");
                }
            });

            $("#chall,.checkall input[type='checkbox']").each(function () {
                if ($(this).attr("checked")) {
                    $(this).addClass("checked");
                }
                else {
                    $(this).removeClass("checked");
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="0" cellpadding="0" style="width: 100%; border: none;" class="msgtable">
            <tr>
                <td style="text-align: right; width: 100px;">
                    模板类型：
                </td>
                <td>
                    <asp:DropDownList ID="mod_type" runat="server" AutoPostBack="true" OnSelectedIndexChanged="typechanged">
                        <asp:ListItem>PC</asp:ListItem>
                        <asp:ListItem>Mobile</asp:ListItem>
                    </asp:DropDownList>
                    <asp:LinkButton ID="refind" runat="server" Text="重新查找" OnClick="rebind"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;" rowspan="2">
                    文件路径：
                </td>
                <td>
                    模板目录：<asp:Label ID="input_path" runat="server" Text="/temple/"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    输出目录：
                    <asp:Label ID="output_path" runat="server" Text="/www/"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink runat="server" ID="toview" Target="_blank"
                        NavigateUrl="/www/">预览网站</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    使用标签对：
                </td>
                <td>
                    <asp:RadioButtonList ID="currenttagpair" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="tagTypeCHange">
                        <asp:ListItem>{@…/}</asp:ListItem>
                        <asp:ListItem Selected="True">{#…/}</asp:ListItem>
                        <asp:ListItem>{$…/}</asp:ListItem>
                        <asp:ListItem>{%…/}</asp:ListItem>
                        <asp:ListItem>{&…/}</asp:ListItem>
                        <asp:ListItem>{…/}不推荐</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    标签说明：
                </td>
                <td>
                    <div style="color: Red;">
                        <asp:Label ID="tagInfo" runat="server"></asp:Label></div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    标签转换：
                </td>
                <td>
                    <asp:CheckBox ID="isconvert" runat="server" OnCheckedChanged="isConvert" AutoPostBack="true" />
                    <span style="margin-left: 50px;" id="doConvert" runat="server" visible="false">选择【目标标签对】：<asp:RadioButtonList
                        RepeatLayout="Flow" ToolTip="新的标签形式" ID="toTagpairs" RepeatDirection="Horizontal"
                        runat="server">
                        <asp:ListItem>{@…/}</asp:ListItem>
                        <asp:ListItem>{#…/}</asp:ListItem>
                        <asp:ListItem>{$…/}</asp:ListItem>
                        <asp:ListItem>{%…/}</asp:ListItem>
                        <asp:ListItem>{&…/}</asp:ListItem>
                        <asp:ListItem>{…/}不推荐</asp:ListItem>
                    </asp:RadioButtonList>
                    </span>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    页面列表：
                </td>
                <td>
                    <table cellspacing="0" cellpadding="0" style="width: 100%; border: none;" class="msgtable">
                        <tr>
                            <th style="text-align: left;">
                                <asp:CheckBox ID="chall" Text="页面名" runat="server" />
                            </th>
                            <th style="text-align: left;">
                                路径
                            </th>
                             <th style="width: 175px;">
                                查看
                            </th>
                            <th style="width: 75px;">
                                扩展名
                            </th>
                            <th style="width: 150px;">
                                大小
                            </th>
                            <th style="width: 180px;">
                                修改日期
                            </th>
                        </tr>
                        <asp:Repeater ID="pages" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:CheckBox ID="cb_id" CssClass="checkall" runat="server" Text='<%# toPageNaem(Eval("Name"))%>' />
                                    </td>
                                    <td style="text-align: left;">
                                        <%# Eval("_path")+""+ Eval("Name")%>
                                    </td>
                                    <td style="text-align: center;">
                                        <a href='<%#Eval("www")%>' target="_blank">
                                            <%#Eval("www")%></a>
                                    </td>
                                    <td style="text-align: center;">
                                        <%#Eval("Extension")%>
                                    </td>
                                    <td style="text-align: center;">
                                        <%#string.Format("{0:0.00}", Eval("Length"))%>KB
                                    </td>
                                    <td style="text-align: center;">
                                        <%#Eval("LastWriteTime")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="4">
                                <asp:Literal ID="pageinfo" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;" rowspan="2">
                    操作：
                </td>
                <td>
                    <asp:CheckBox ID="clearBefor" runat="server" Text="先清空输出文件夹" ToolTip="在生成之前删除输出目录下的所有\.aspx\.ascx\.cs\.ashx 文件" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox
                        ID="showTagInfos" runat="server" Text="显示解析结构" />
                </td>
            </tr>
            <tr>
                <td>
                    <div style="padding: 5px; margin-left: 80px;">
                        <asp:Button ID="make" runat="server" OnClick="doMake" Text="生成选中" CssClass="button-submit" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <code style="padding: 5px; margin-left: 10px;">信息：
                        <asp:Literal ID="makeresult" runat="server" Text="暂无"></asp:Literal></code>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
