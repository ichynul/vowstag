<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tables.aspx.cs" Inherits="_Tables" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>表结构</title>
    <script src="js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#VLine").height($(window).height());
            $("#fieldDiv").width($(window).width() - $("#tablesDiv").width() - 15);
            $("#fieldDiv").height($(window).height() - 2);
            $("#fieldstable").height($(window).height() - 30);
            $("#tablesDiv").height($(window).height() - 20);
        });
    </script>
    <style type="text/css">
        body
        {
            padding: 0;
            margin: 0;
        }
        
        table
        {
            border: none;
        }
        
        a
        {
            text-decoration: none;
            color: #79a1b5;
        }
        
        #tablesDiv
        {
            padding: 5px;
            width: 150px;
            overflow: auto;
            border-right: 1px dotted #0094ff;
        }
        
        #fieldstable
        {
            width: 100%;
            position: absolute;
            overflow: auto;
        }
        
        #tablesDiv a, #tablesDiv label
        {
            font-size: 12px;
            overflow-wrap: break-word;
            word-wrap: break-word;
            float: left;
        }
        
        #tables input
        {
            float: left;
        }
        
        #tables
        {
            width: 100%;
        }
        
        #fieldDiv
        {
            position: absolute;
            left: 163px;
            top: 0px;
            background-color: #f8f8f8;
            background-color:#e1e1e1;
        }
        
        #tables td
        {
            overflow: hidden;
            overflow-wrap: break-word;
        }
        
        #info
        {
            width: 100%;
            position: absolute;
            height: 30px;
            z-index: 999;
            top: 0px;
        }
        
        
        #fieldstable table
        {
            width: 100%;
        }
        
        #info input#go
        {
            margin: 3px;
            height: 24px;
        }
        
        #fieldstable th, #fieldstable td
        {
            text-align: center;
            display: table-cell;
            empty-cells: show;
            white-space: nowrap;
            overflow-wrap: break-word;
            overflow: hidden;
            background-color: #fff;
        }
        #fieldstable td
        {
            height: 25px;
            font-size: 12px;
            border-bottom: 1px dotted #c2c2c2;
            border-right: 1px dotted #c2c2c2;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="tablesDiv">
        <div style="padding-left: 16px;">
            <label>
                选择表：</label><asp:LinkButton ID="reTables" runat="server" OnClick="refreshTables"
                    ToolTip="重新读取表数据及其字段信息，会重置所有设置！">刷新</asp:LinkButton>
        </div>
        <asp:RadioButtonList CellPadding="0" CellSpacing="0" BorderWidth="0" ID="tables" runat="server" RepeatDirection="Vertical" AutoPostBack="true"
            OnSelectedIndexChanged="ReBindTableFields">
        </asp:RadioButtonList>
    </div>
    <div id="fieldDiv">
        <div id="fieldstable">
            <table>
                <thead>
                    <tr>
                        <th style="width: 200px;" class="head">
                            字段名称
                        </th>
                        <th style="width: 160px;" class="head">
                            字段类型
                        </th>
                        <th style="width: 60px;" class="head">
                            NullAble
                        </th>
                        <td>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="taableFields" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: center; padding-left: 3px;">
                                    <div style="width: 200px; white-space: nowrap; overflow: hidden;">
                                        <%#Eval("Name") %>
                                    </div>
                                </td>
                                <td class="dtype">
                                    <%#Eval("Type") %>
                                </td>
                                <td>
                                    <span class="nullable" title="NullAble"><%#Eval("NullAble") %></span>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
