<%@ Page Language="C#" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="Page_list" ValidateRequest="true" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!-- Powered by VowsTag http://git.oschina.net/ichynul/vowstag/wikis/home -->

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><% = config.webname %></title>
    <!--引入公共样式、脚本-->
    <link href="/temple/page/css/style.css" rel="stylesheet" type="text/css" />
<script src="/temple/page/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<meta name="keywords" content="<% = config.keyword %>" />

</head>
<body>
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_16_3" runat="server"></asp:PlaceHolder><!--引入label--></div>
    <div class="main">
        <div class="articles">
            <!--局部的read-->
            <div>
                <a class="catename"><% =ReadTag_31_4.Name %><!--栏目名称-->
                </a>
            </div>
            
            <ul class="articlelist">
                
<asp:Repeater ID="ListTag_73_6" runat="server">
    <ItemTemplate><!--每页显示4条->
                <!--list中的list.本标签中引用了url参数(categ= request.cid) -->
                <li><a class="title" href="read.aspx?id=<%# Eval("id") %>"><%# Eval("title") %></a> <span class="time">
                    <%# Eval("time") %></span>
                    <div class="desc">
                        <span><%# SubString(Eval("content"),200) %></span></div>
                       <img src="<%# Eval("img") %>" style=" width:100px; height:100px; float:right" alt='' />
                </li>
                </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_73_6" runat="server"></asp:Literal>
            </ul>
            <asp:Literal ID="PagerTag_17_12" runat="server"></asp:Literal><!--分页标签 type=cs为cs代码直接输出分页html,type=js则会输出分页需要的参数-->
        </div>
    </div>
    <div class="foot">
    <ul class="copyleft">
        <li><a href="index.aspx"><% = config.webname %></a></li>
        <li><% = config.beianhao %></li>
        <li><% = config.copyleft %></li>
        <li><% = config.keyword %></li>
    </ul>
</div>
<!--引入static-->

<form id="form1" runat="server">
    <script type="text/javascript">
        /* ajax 脚本支持 Created by VowsTag */
        var _tagcall = {
            //发起请求. arg以'key1=value1&key2=value2..'的形式组成键值对,服务端重写public override CallBackResult DoCallBack()以处理请求。
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
