<%@ Page Language="C#" AutoEventWireup="true" CodeFile="read.aspx.cs" Inherits="Page_read" ValidateRequest="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!-- Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home -->

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><% =read.Title %>--<% = config.webname %></title>
    <!--引入公共样式、脚本-->
    <link href="/temple/page/css/style.css" rel="stylesheet" type="text/css" />
<script src="/temple/page/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<meta name="keywords" content="<% = config.keyword %>" />

</head>
<body>
        <!-- CMDTag_37577_4 -->
    <!--指定页面处理类 -->
        <!-- ReadTag_30100_5 --><!--单标签形式的read,可以在页面任何位置读取其read字段值；每个页面最多只能有一个全局read-->
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_16100_6" runat="server"></asp:PlaceHolder><!--引入label-->
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
                        &gt;<a href="list.aspx?cid=<% =ReadTag_30654_9.ID %>&page=<% = page %>"><% =ReadTag_30654_9.Name %></a><!--局部的read-->
                        &gt;<a><% =read.Title %></a>
        </div>
        <div class="read">
            <div class="infos">
                <h3>
                    <% =read.Title %>
                </h3>
                <span>作者：<% =read.Author %></span><span>日期：<% = TimeFormat(read.Time) %></span><span>点击：<% =read.View %></span>
            </div>
            <img src="<% =read.Img %>" style="width: 250px; margin-left: 250px;" />
            <div class="content">
                <% =read.Content %>
            </div>
            <div class="links">
                <span>上一篇： </span>
                <!--用list标签读取上一篇-->
                
<asp:Repeater ID="ListTag_71177_20" runat="server">
    <ItemTemplate><!--此处用字段id与url参数比较 -->
                                <a href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a>
                                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_71177_20" runat="server"></asp:Literal>

                                <span>下一篇：</span>
                <!--用list标签读取下一篇-->
                
<asp:Repeater ID="ListTag_55698_23" runat="server">
    <ItemTemplate>
                <!--此处用字段id与read.id(read标签的一个字段)比较,这里跟request.id差不多-->
                                <a href="read.aspx?id=<%# Eval("ID") %>"><%# Eval("Title") %></a>
                                
                                </ItemTemplate>
</asp:Repeater>
            <% if (ListTag_55698_23Empty) %>
<% { %>
<!--empty标签对，作用与emptytext差不多，但emptytext仅支持简单的文字，而empty可包含复杂的文本（如html） -->
                                <span style="color: Red;">没有了~</span> <% = Request.QueryString["id"] %>
                                
<% } %>

                            </div>
            <div class="ding">
                <a id="dingyixia" style="color: Red; font-size: 15px;" href="javascript:;" onclick='ding()'>
                    顶一个(<% =read.ding %>)</a>
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
    <script type="text/javascript">
        function ding() {
            $('#dingyixia').text('操作中...');
            _tagcall.$('action=ding&id=<% = Request.QueryString["id"] %>');
        }

        function CallbackSuccess(resultstr, context) {
            var obj = eval('(' + resultstr + ')');
            if (obj.type) {
                if (obj.type == "mycall") {//自定义请求的返回
                    $('#dingyixia').text('顶一个(' + obj.result.ding + ')');
                    alert(obj.result.msg);
                }
                else if (obj.type == "formcall") {//form标签请求的返回

                }
                else if (obj.type == "jsoncall") {//json标签请求的返回

                }
                else {
                    alert(resultstr);
                }
            }
            else {
                alert(resultstr);
            }
        }
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
                this.$('jsonname=' + jsonname + '&' + otenparams ,async ,jsonname);
            }
        };
        _tagcall.$json =_tagcall.json;//   _tagcall.$json 将弃用
        var _tagcallback = _tagcall;//兼容旧版
    </script>
</form>
</body>
</html>
