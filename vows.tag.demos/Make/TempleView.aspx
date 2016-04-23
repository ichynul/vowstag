<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TempleView.aspx.cs"
    Inherits="ServerFilePath" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="renderer" content="webkit|ie-comp|ie-stand">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta http-equiv="Cache-Control" content="no-siteapp" />
    <style type="text/css">
        #pnlTreeview {
            height: 100%;
            width: 200px;
            overflow: auto;
            float: left;
        }

        #viewdiv {
            float: left;
            height: 100%;
            position: absolute;
            left: 210px;
            top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="editable" runat="server" />
        <div>
            <asp:Panel ID="pnlTreeview" runat="server" ScrollBars="Auto">
                <asp:TreeView ID="treeFileView" runat="server" ImageSet="XPFileExplorer" NodeIndent="15"
                    ShowLines="True" OnSelectedNodeChanged="treeFileView_SelectedNodeChanged">
                    <ParentNodeStyle Font-Bold="False" />
                    <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                    <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                        VerticalPadding="0px" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
                        NodeSpacing="0px" VerticalPadding="2px" />
                </asp:TreeView>
            </asp:Panel>
            <div id="viewdiv" runat="server">
            </div>
        </div>
    </form>
    <script src="js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#viewdiv").width($(window).width() - 260);
        });
    </script>
</body>
</html>
