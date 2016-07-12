<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="Page_test" EnableViewState="false" %>

<!-- Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag -->

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    
<asp:Repeater ID="ListTag_24570_1" runat="server">
    <ItemTemplate><!--不分页3条-->
    <li><a class="title" href="formTag.aspx?id=<%# Eval("ID") %>"><%# SubString(Eval("Title"),15) %></a>
        <span class="time"><%# Eval("Time") %></span> </li>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_24570_1" runat="server"></asp:Literal>

    <hr />
    <!--label中的read  读取那条ID为4的文章-->
    <a style="color: Red;" href="read.aspx?id=<% =ReadTag_1719_5.ID %>"><% =ReadTag_1719_5.Title %><% = SubString(ReadTag_1719_5.Title,15) %><!--限制标题最多15个字--></a>
<asp:Repeater ID="ListTag_57550_9" runat="server">
    <ItemTemplate><!--不分页3条-->
    <li><a class="title" href="formTag.aspx?id=<%# Eval("ID") %>"><%# SubString(Eval("Title"),15) %></a>
        <span class="time"><%# Eval("Time") %></span> </li>
    </ItemTemplate>
</asp:Repeater>
<asp:Literal ID="empty_ListTag_57550_9" runat="server"></asp:Literal>
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
