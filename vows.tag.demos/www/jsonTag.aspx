<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jsonTag.aspx.cs" Inherits="Page_jsonTag" EnableViewState="false" %>

<!--2016年08月31日 09:38:20 Powered by VowsTag v-1.4.16.8 http://git.oschina.net/ichynul/vowstag -->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><% = config.webname %></title>
    <!--引入公共样式、脚本-->
    <link href="/temple/www/page/css/style.css" rel="stylesheet" type="text/css" />
<script src="/temple/www/page/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<meta name="keywords" content="<% = config.keyword %>" />

    <link href="/temple/www/page/css/kkpager_orange.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_16272_3" runat="server"></asp:PlaceHolder><!--引入label--></div>
    <div class="navi">
        <ul class="navilist">
    <li><a href="/">首页</a></li>
    <li><a href="listTag.aspx">listTag</a></li>
    <li><a href="readTag.aspx">ReadTag</a></li>
    <li><a href="jsonTag.aspx">jsonTag</a></li>
    <li><a href="formTag.aspx">formTag</a></li>
    <li><a style="color: Red; font-size: 16px;" href="/Make/Maker.aspx">后台管理&gt;&gt;</a></li>
</ul>

    </div>
    <div class="main">
        <div class="location">
            当前位置：<a href="index.aspx"><% = config.webname %></a>
                        &gt;jsonTag
                    </div>
        <div class="categs">
            <ul class="categslist" id="switch">
                <li><a href="javascript:;" onclick="change(this,'');" class="on">全部</a></li>
                
<asp:Repeater ID="ListTag_45501_6" runat="server">
    <ItemTemplate>
                <li><a href="javascript:;" onclick="change(this,'<%# Eval("ID") %>');"><%# Eval("Name") %></a></li>
                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_45501_6" runat="server"></asp:Literal>

            </ul>
        </div>
        <div class="articles">
            <ul class="articlelist" id="thelist">
            </ul>
            <div id="kkpager" class="tcdPageCode">
            </div>
        </div>
    </div>
    <div class="foot">
        <ul class="copyleft">
    <li><a href="index.aspx"><% = config.webname %></a></li>
    <li><% = config.beianhao %></li>
    <li><% = config.copyleft %></li>
    <li><% = config.keyword %></li>
</ul>
<!--引入static-->
    </div>
    <script src="/temple/www/page/js/kkpager.min.js" type="text/javascript"></script>
    <script src="/temple/www/page/js/doT.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            loadMore(); //页面加载完事自动获取第一页的数据
        });

        var backpage = '<% = Request.QueryString["backpage"] %>';
        var page = 1, pagesize;
        if (backpage != '') {
            page = parseInt(backpage);
        }
        var pages, listsize
        var type = '';
        var over = false;

        function loadMore() {
            var jsonname = "JsonTag_82275_11";
            //fields = title,time,desc,img,id 读指定读取的字段，节省流量。若不指定将读取所有字段
            if (type != '') { //按具体分类
                jsonname = "JsonTag_85173_12";
                //categ = call.cid 中 call.cid为一个占位符(类似于request.xxx) 代表call参数中的cid值
            }
            _tagcall.json(jsonname, "page=" + page + '&cid=' + type); ///  在这里指定cid的值
        }

        function CallbackSuccess(resultstr) {
            var obj = eval('(' + resultstr + ')');
            if (obj.type) {
                if (obj.type == "jsoncall") {//json标签请求的返回
                    if (obj.result && obj.result.data) {
                        over = obj.result.over;
                        page = obj.result.page;
                        listsize = obj.result.listsize;
                        pagesize = obj.result.pagesize;
                        paging();
                        if (obj.result.data.length > 0) {
                            var tpl = $('#list-template').text();
                            var tempFn = doT.template(tpl);
                            $("#thelist").html(tempFn(obj.result.data));
                        }
                        else {
                            if (obj.result.page == 1) {
                                $("#thelist").html('<li><a style="text-align:center; width:100%;padding-bottom:20px;">暂无文章</a></li>');
                            }
                            else {
                                alert('没有更多了~');
                            }
                        }
                    }
                }
                else if (obj.type == "mycall") {

                }
                else {
                    alert(resultstr);
                }
            }
            else {
                alert(resultstr);
            }
        }

        function paging() {
            pages = Math.floor(listsize / pagesize); //获取当前的总页数
            if (listsize % pagesize != 0) {
                pages += 1;
            }
            pages = pages < 1 ? 1 : pages;
            page = page < 1 ? 1 : page > pages ? pages : page;

            kkpager.generPageHtml({
                pno: page,
                //总页码
                total: pages,
                //总数据条数
                totalRecords: listsize,
                mode: 'click', //默认值是link，可选link或者click
                click: function (n) {
                    page = n;
                    loadMore();
                    this.selectPage(n);
                }, getHref: function (n) {
                    return 'javascript:';
                }
                , isGoPage: false
                , isShowTotalPage: false
                , isShowCurrPage: false
                /*
                ,lang				: {
                firstPageText			: '首页',
                firstPageTipText		: '首页',
                lastPageText			: '尾页',
                lastPageTipText			: '尾页',
                prePageText				: '上一页',
                prePageTipText			: '上一页',
                nextPageText			: '下一页',
                nextPageTipText			: '下一页',
                totalPageBeforeText		: '共',
                totalPageAfterText		: '页',
                currPageBeforeText		: '当前第',
                currPageAfterText		: '页',
                totalInfoSplitStr		: '/',
                totalRecordsBeforeText	: '共',
                totalRecordsAfterText	: '条数据',
                gopageBeforeText		: ' 转到',
                gopageButtonOkText		: '确定',
                gopageAfterText			: '页',
                buttonTipBeforeText		: '第',
                buttonTipAfterText		: '页'
                }*/
            }, true);
        }

        function change(obj, t) {
            $("#thelist").html('');
            $("#switch li a").removeClass("on");
            $(obj).addClass("on");
            type = t;
            over = false;
            page = 1;
            loadMore();
        }

    </script>
    <script id="list-template" type="text/x-dot-template">
        {{ for(var i=0, len=it.length; i < len; i++) {}}
            <li>
                <a class="title" href="formTag.aspx?id={{=it[i].ID}}&page={{=page}}">{{=it[i].Title}}</a>
                <span class="time">{{=it[i].Time}}</span>
                <div class="desc">
                    <span>{{=it[i].Desc}}</span></div>
            </li>
        {{ } }}
    </script>

<form id="form1" runat="server">
    <script id="_tagcall" type="text/javascript" src="/temple/www/page/js/_tagcall.js" ></script>
</form>
</body>
</html>
