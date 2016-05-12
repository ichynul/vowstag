<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPage.aspx.cs" Inherits="WebMana_Temple_ViewPage"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="renderer" content="webkit|ie-comp|ie-stand">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta http-equiv="Cache-Control" content="no-siteapp" />
    <style type="text/css">
        #viewdiv
        {
            height: 100%;
            overflow: auto;
        }
        
        table, th, td
        {
            border: none;
        }
        
        .msgtable
        {
            width: 100%;
        }
        
        #Content
        {
            width: 95%;
            height: 80%;
        }
        
        body
        {
            margin: 0;
            padding: 0;
        }
        #tagList
        {
            float: left;
            width: 100%;
            height: 200px;
            overflow:auto;
        }
        #tagList li
        {
            float: left;
            padding: 3px;
            list-style: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="editable" runat="server" />
    <div>
        <div id="viewdiv" runat="server">
            <table class="table table-border table-bordered table-bg table-hover table-sort">
                <tr>
                    <th>
                        当前文件/文件：<asp:Label ID="Labelname" runat="server"></asp:Label>
                    </th>
                </tr>
                <tbody id="tagArea" runat="server" visible="false">
                    <tr>
                        <td>
                            使用标签对：
                            <asp:RadioButtonList ID="currenttagpair" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="tagTypeCHange">
                                <asp:ListItem>{…/}</asp:ListItem>
                                <asp:ListItem>{@…/}</asp:ListItem>
                                <asp:ListItem Selected="True">{#…/}</asp:ListItem>
                                <asp:ListItem>{$…/}</asp:ListItem>
                                <asp:ListItem>{%…/}</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            标签：
                            <asp:Label ID="tagList" runat="server"></asp:Label>
                        </td>
                    </tr>
                </tbody>
                <tr runat="server">
                    <td>
                        <asp:TextBox ID="Content" runat="server" CssClass="required" Columns="100" Rows="70"
                            TextMode="MultiLine" Height="600px" Width="98%"></asp:TextBox>
                        <img id="imgview" style="max-width: 600px; margin: 5px;" onclick="window.open(this.src,'_blank')"
                            runat="server" visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="margin-top: 10px; text-align: center;">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary radius" OnClick="Button1_Click"
                                Text="保存 " />
                            <button id="Button2" onclick="UnDo();" runat="server" class="btn btn-default radius"
                                type="button">
                                取消</button>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
    <script src="js/edit_area_loader.js" type="text/javascript"></script>
    <script src="js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function UnDo() {
            if (confirm('你确定要取消所做的更改吗?')) {
                document.getElementById('fromeditor').reset();
            }
        }
        function getValue(value) {
            if (value != "")
                insert(value);
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $("#viewdiv").width($(window).width() - 10);
            $("#viewdiv").height($(window).height() - 50);
            $("#Content").width($(window).width() - 40);
            $("#Content").height($(window).height() - 220 - $('#tagList').height());
            if ($("#editable").val() == "yes") {
                editAreaLoader.init({
                    id: "Content"	// id of the textarea to transform		
			, start_highlight: true	// if start with highlight
			, allow_resize: "both"
			, allow_toggle: true
			, word_wrap: true
            , toolbar: "search, go_to_line,fullscreen,|, undo, redo, |, select_font, |, syntax_selection, |, change_smooth_selection, highlight, reset_highlight, |, help"
			, syntax_selection_allow: "basic,brainfuck,c,coldfusion,cpp,css,html,java,js,pas,perl,php,python,ruby,robotstxt,sql,tsql,vb,xml"
			, language: "zh"
			, syntax: "html"
                });
            }
        });

    </script>
</body>
</html>
