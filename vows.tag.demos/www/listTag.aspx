<%@ Page Language="C#" AutoEventWireup="true" CodeFile="listTag.aspx.cs" Inherits="Page_listTag" EnableViewState="false" %>

<!-- Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag -->

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
        <asp:PlaceHolder ID="LabelTag_16790_3" runat="server"></asp:PlaceHolder><!--引入label-->
    </div>
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
                        &gt;ListTag
                    </div>
        <div class="articles">
            <p class="about">
                分页 默认会以第一个字段、降序排列，分页3条</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_2693_6" runat="server">
    <ItemTemplate><!--分页3条-->
                <li><a class="title" href="formTag.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
                    <%# Eval("Time") %></span> </li>
                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_2693_6" runat="server"></asp:Literal>

            </ul>
            <div id="kkpager" class="tcdPageCode">
            </div>
            <!-- 分页标签，一个页面最多只能有一个分页的list，最多一个分页标签-->
            <asp:Literal ID="PagerTag_17306_10" runat="server"></asp:Literal><!--tpey=js，前端分页(可以用第三方分页实现个性化分页)，会在标签出输出分页需要的隐藏信息 -->
            <div>
                <h4>
                    ----输出分页信息----</h4>
                结果集总大小(list_size)：<span id="_list_size"></span><br />
                每页显示条数(num_per_page)：<span id="_num_per_page"></span><br />
                当前页码(current_page)：<span id="_current_page"></span><br />
                url参数(current_url_params)：<span id="_current_url_params"></span><br />
                计算总页数：<span id="pages"></span><br />
                <span id="pagerlinks"></span>
                <h4>
                    ----输出分页信息----</h4>
                <script src="/temple/page/js/kkpager.min.js" type="text/javascript"></script>
                <script type="text/javascript">
                    $(function () {
                        $('#_list_size').text($('#list_size').val());
                        $('#_num_per_page').text($('#num_per_page').val());
                        $('#_current_page').text($('#current_page').val());
                        var url = $('#current_url_params').val();
                        $('#_current_url_params').text(url);
                        var liszie = parseInt($('#list_size').val());
                        var per_page = parseInt($('#num_per_page').val());
                        var pages = Math.floor(liszie / per_page);
                        if (liszie % per_page != 0) {
                            pages += 1;
                        }
                        pages = pages < 1 ? 1 : pages;
                        $('#pages').text(pages);
                        if ($('#current_page').val() != '') {
                            var page = parseInt($('#current_page').val());
                        }
                        else {
                            page = 1;
                        }
                        page = page < 1 ? 1 : page > pages ? pages : page;
                        kkpager.generPageHtml({
                            pno: page,
                            //总页码
                            total: pages,
                            //总数据条数
                            totalRecords: liszie,
                            mode: 'click', //默认值是link，可选link或者click
                            click: function (n) {
                                page = n;
                                location.href = url + 'page=' + page;
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

                        //分页，
                    });
                </script>
            </div>
        </div>
        <div class="articles">
            <p class="about">
                以下list标签均读取article表的数据，且显示的样式一样，这时可以把样式放到一个文件里面，然后用 item=filename 引用，避免重复书写，维护方便</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_411_11" runat="server">
    <ItemTemplate><li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
    <%# Eval("Time") %></span> </li>
</ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_411_11" runat="server"></asp:Literal>

                <!--读一条看看。因为不需要包含样式，所以以单标签的形式-->
                <!--item=listTag_item 样式文件位于 /item/listTag_item.html(文件扩展名可以是.html/.htm/.txt)-->
            </ul>
        </div>
        <div class="articles">
            <p class="about">
                逻辑且'&',查询条件之间用&分割表示逻辑且(短路的，实际效果为&&,书写的时候不要写成&&),逻辑关系与标签指令（orderby、desc、take、skip、item等）无书写位置的限定</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_54869_12" runat="server">
    <ItemTemplate><li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
    <%# Eval("Time") %></span> </li>
</ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_54869_12" runat="server"></asp:Literal>

                <!--id>=3    id大于等于3-->
                <!--id<999   且id小于99-->
                <!--<br />   替代一个&，以使标签分行(当用编辑器格式化时)-->
            </ul>
        </div>
        <div class="articles">
            <p class="about">
                逻辑或'|'(短路的，实际效果为||),</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_77438_13" runat="server">
    <ItemTemplate><li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
    <%# Eval("Time") %></span> </li>
</ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_77438_13" runat="server"></asp:Literal>

                <!--id=3    id等于3-->
                <!--id=6    或id等于6-->
                <!--<hr />   替代一个|，以使标签分行-->
                <!--id<=15&id>12    或id小于等于15且id大于10,该条件没有接着前两个条件写（被take指令分割），不影响结果，但应避免这样，最好先写逻辑，再写指令-->
                <!--take=5 不出意外的话会有5条数据，若其中有锁定的（IsLock为true），则会少于5-->
            </ul>
        </div>
        <div class="articles">
            <p class="about">
                文本包含'%'、不包含'!%'</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_66475_14" runat="server">
    <ItemTemplate><li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
    <%# Eval("Time") %></span> </li>
</ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_66475_14" runat="server"></asp:Literal>

                <!--titel%人          标题不包含'O'字母-->
                <!--content!%[二 B]    正文不包含"二 B"，此处文字外有[]（文字中有空格的，要用引号或[]包围，否则可以省去）-->
                <!--title!%你 好      这个不对，文字中间的空格会不在了，实际效果为 title!%你好 -->
                <!--%和!%只对文字(string)有效，其他类型的字段不可以。另外要注意字段不能为空，若表中该字段存在NULL值，则会出错。应判断空值，如(title!=null&title%人) -->
            </ul>
        </div>
        <div class="articles">
            <p class="about">
                被包含'#'、不被包含'!#'  </p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_89204_15" runat="server">
    <ItemTemplate><li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
    <%# Eval("Time") %></span> </li>
</ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_89204_15" runat="server"></asp:Literal>

                <!--titel !# "你好,hello,hi"          标题不是 你好,hello,hi 中的任意一个-->
                <!--id #[11,13,15,17]   id 为 1,2,3,4 中的任意一个-->
                <!--数组用双引号""或[]包围-->
            </ul>
        </div>
        <div class="articles">
            <p class="about">
                记录为空简单文本提示</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_60479_16" runat="server">
    <ItemTemplate><li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
    <%# Eval("Time") %></span> </li>
</ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_60479_16" runat="server"></asp:Literal>

                <!--emptytext支持简单的纯文本 -->
            </ul>
        </div>
        <div class="articles">
            <p class="about">
                记录为空复杂文本提示</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_20588_17" runat="server">
    <ItemTemplate><!--不使用item外部文件样式-->
                <li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
                    <%# Eval("Time") %></span> </li>
                                </ItemTemplate>
</asp:Repeater>
            <% if (ListTag_20588_17Empty) %>
<% { %>
                <li>没有id小于<b style="color: Red;">0</b>的记录 ~!~ </li>
                
<% } %>

                <!--empty标签支持任意复制文本 -->
            </ul>
        </div>
        <div class="articles">
            <p class="about">
                时间，支持DateTime.Now的形式</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_76227_21" runat="server">
    <ItemTemplate><li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
    <%# Eval("Time") %></span> </li>
</ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_76227_21" runat="server"></asp:Literal>

                <!--time>2015/12/25 12:25:54          录入日期 在2015/12/25 12:15:54之后的。时间用'-'或'/'分割-->
                <!--time < DateTime.Now               今天之后后的-->
            </ul>
        </div>
        <div class="articles">
            <p class="about">
                session、cookie、后端变量</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_107270_22" runat="server">
    <ItemTemplate><li><a class="title" href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a> <span class="time">
    <%# Eval("Time") %></span> </li>
</ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_107270_22" runat="server"></asp:Literal>

                <!--uid=session.uid           与session值比较-->
                <!--uid=user.ID               与后端变量比较，注意字段区分大小写-->
                <!--author=cookie.uname       与cookie比较-->
                <!--author=cookie.user.uname  与二级cookie比较-->
            </ul>
        </div>
        <div class="articles">
            <p class="about">
                受保护的表</p>
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_22603_23" runat="server">
    <ItemTemplate><!--受保护的表'Admin'，无法使用-->
                <li>用户名：<%# Eval("Username") %><br />
                    密码：<%# Eval("Password") %></li>
                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_22603_23" runat="server"></asp:Literal>

            </ul>
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
                if (typeof(window.console) != undefined)
                {
                    window.console.debug( '出错了！--' + error );
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
                this.$('jsonname=' + jsonname + '&' + (otenparams || '') ,async ,jsonname);
            }
        };
        _tagcall.$json =_tagcall.json;//   _tagcall.$json 将弃用
        var _tagcallback = _tagcall;//兼容旧版
    </script>
</form>
</body>
</html>

