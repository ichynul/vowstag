<%@ Page Language="C#" AutoEventWireup="true" CodeFile="web.aspx.cs" Inherits="Make_index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>vows.tag</title>
    <script src="./Make/js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <link href="./Make/css/style.css" rel="stylesheet" type="text/css" />
    <script>
        $(function () {
            $('#right').width($(window).width() - $('#left').width() - 10);
            $('#left a').click(function () {
                $('#left a').removeClass('selectd');
                $(this).addClass('selectd');
            });
        });
    </script>
    <style>
        #left li
        {
            margin-left: 10px;
            padding: 1px;
            border-bottom: 1px dashed #999;
        }
        #left a
        {
            line-height: 20px;
            color: #777;
            text-decoration: none;
        }
        #left a.selectd
        {
            color: Orange;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="left" style="width: 160px; position: absolute; left: 0px;">
            <ul>
                <li><a class="selectd" href="./Make/Maker.aspx" target="rightwindow">页面管理</a></li>
                <li><a href="./Make/TempleView.aspx" target="rightwindow">模板编辑 </a></li>
                <li><a href="./Make/Tables.aspx" target="rightwindow">查看表结构 </a></li>
            </ul>
        </div>
        <div id="right" style="height: 98%; position: absolute; right: 0px; border-left: 1px solid #888;
            float: left;">
            <iframe id="rightwindow" name="rightwindow" scrolling="yes" src="./Make/Maker.aspx" style="height: 100%;
                width: 100%; z-index: 1;" frameborder="0" marginheight="0"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
