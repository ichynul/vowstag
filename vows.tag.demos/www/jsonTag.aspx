<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jsonTag.aspx.cs" Inherits="Page_jsonTag" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!-- Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home -->

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><% = config.webname %></title>
    <!--引入公共样式、脚本-->
    <link href="/temple/page/css/style.css" rel="stylesheet" type="text/css" />
<script src="/temple/page/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<meta name="keywords" content="<% = config.keyword %>" />

    <link href="/temple/page/css/kkpager_orange.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_16635_3" runat="server"></asp:PlaceHolder><!--引入label--></div>
    <div class="navi">
        <ul class="navilist">
    <li><a href="/">首页</a></li>
    <li><a href="listTag.aspx">listTag</a></li>
    <li><a href="readTag.aspx">ReadTag</a></li>
    <li><a href="jsonTag.aspx">jsonTag</a></li>
    <li><a href="formTag.aspx">formTag</a></li>
    <li><a style="color: Red; font-size: 16px;" href="/Make/index.aspx">后台管理&gt;&gt;</a></li>
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
                
<asp:Repeater ID="ListTag_45964_6" runat="server">
    <ItemTemplate>
                <li><a href="javascript:;" onclick="change(this,'<%# Eval("ID") %>');"><%# Eval("Name") %></a></li>
                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_45964_6" runat="server"></asp:Literal>

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
    <script src="/temple/page/js/kkpager.min.js" type="text/javascript"></script>
    <script src="/temple/page/js/doT.min.js" type="text/javascript"></script>
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
            var jsonname = "JsonTag_71964_11";
            //fields = title,time,desc,img,id 读指定读取的字段，节省流量。若不指定将读取所有字段
            if (type != '') { //按具体分类
                jsonname = "JsonTag_85964_12";
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
    <script type="text/javascript">
        /* ajax 脚本支持 Created by VowsTag */
        var _tagcall = {
            //发起请求. arg以'key1=value1&key2=value2..'的形式组成键值对,服务端重写public override CallbackResult DoCallback()以处理请求。
            //用方法:this.CallValue("key1")可获取value1',context参数可省略.(详细请了解asp.net的'callback'机制)
            $: function (arg ,async ,context)
            {
                //请求发起前预处理方法，一般显示一个提示
                if (typeof(BeforCallback) == 'function' )
                {
                    BeforCallback();
                }
                if (async != false)//异步
                {
                    <% = ClientScript.GetCallbackEventReference(this ,"arg" ,"this.success" ,"this.error" ,"context" ,true) %>;
                }
                else
                {
                    <% = ClientScript.GetCallbackEventReference(this ,"arg" ,"this.success" ,"this.error" ,"context" ,false) %>;
                }
            },
            success: function (result ,context)//异步发起请求成功
            {
                //看是否有处理的方法
                if (typeof(CallbackSuccess) == 'function' )
               {
                    CallbackSuccess(result ,context);
                }
                else
                {
                    alert('服务端返回：\r\n' + result+'\r\n请实现js方法CallbackSuccess(result)以处理返回结果。');
                }
            },
            error: function (error ,context)//异步发起请求错误
            {
                if (window.console);
                {
                    window.console.log( '出错了！--' + error );
                }
                //看是否有处理的方法
                if (typeof(CallbackError) == 'function' )
                {
                    CallbackError(error ,context);
                }
                else
                {
                    alert('请求出现错误！服务端返回：\r\n' + error+'\r\n请实现js方法CallbackError(result)以处理返回结果。');
                }
            },
            formqstr: '',
            form: function (formname ,async)//form标签发起请求
            {
                var elemArray = theForm.elements;
                this.formqstr ='';
                for (var i = 0; i < elemArray.length; i++) {
                    var element = elemArray[i];
                    var elemType = element.type.toUpperCase();
                    var elemName = element.name;
                    if (elemName) {
                        if ( elemType == 'TEXT' || elemType == 'TEXTAREA'
                             || elemType == 'PASSWORD' || elemType == 'FILE' || elemType == 'HIDDEN')
                        {
                            this.getElemValue(elemName, element.value);
                        }
                        else if (elemType == 'CHECKBOX' && element.checked)
                        {
                            this.getElemValue(elemName, element.value ? element.value : 'On');
                        }
                        else if (elemType == 'RADIO' && element.checked)
                        {
                            this.getElemValue(elemName, element.value);
                        }
                        else if (elemType.indexOf('SELECT') != -1)
                        {
                            for (var j = 0; j < element.options.length; j++) {
                                var option = element.options[j];
                                if (option.selected)
                                {
                                    this.getElemValue(elemName, option.value ? option.value : option.text); }
                                }
                           }
                       }
                  }
                this.$('formname=' + formname + '&' + this.formqstr ,async ,formname);
            },
            getElemValue: function(name ,value) {
                if (name == '__VIEWSTATE' || name == '__VIEWSTATEGENERATOR'||
                        name == '__EVENTTARGET' || name =='__EVENTARGUMENT')
                {
                    return;
                }
                this.formqstr += (this.formqstr.length > 0 ? '&' : '') + name + '=' + value;
            },
            json: function (jsonname ,otenparams ,async)//json标签发起请求
            {
                this.$('jsonname=' + jsonname + '&' + otenparams ,async ,jsonname);
            }
        };
        _tagcall.$json =_tagcall.json;//   _tagcall.$json 将弃用
        var _tagcallback = _tagcall;//兼容旧版
    </script>
</form>
</body>
</html>
