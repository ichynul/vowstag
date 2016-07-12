<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formTag.aspx.cs" Inherits="Page_formTag" EnableViewState="false" %>

<!-- Powered by VowsTag v-1.4.16.711 http://git.oschina.net/ichynul/vowstag -->

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><% =read.Title %>--<% = config.webname %></title>
    <!--引入公共样式、脚本-->
    <link href="/temple/page/css/style.css" rel="stylesheet" type="text/css" />
<script src="/temple/page/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<meta name="keywords" content="<% = config.keyword %>" />

</head>
<body>
    <!-- CMDTag_43761_4 -->
    <!--callback=true指令可以不要，因为页面含有form（或json）标签，将会自动启用clllback支持 -->
    <div class="head">
        <asp:PlaceHolder ID="LabelTag_16105_5" runat="server"></asp:PlaceHolder><!--引入label-->
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
                        &gt;formTag
        </div>
        <div class="form">
            <!-- ReadTag_30665_8 --><!--用read来绑定数据-->
            <!-- FormTag_65559_9 --><!--用form来提交数据，一个页面只能有一个from标签-->
            <!--allowempty=author,view 指定 author、view 两个字段可以为空值(不填)，不指定的字段默认不允许为空 -->
            <!--action =edit 执行 '修改'有则修改,无则返回错误。该参数可选值 both[默认]（添加/修改）|add（只添加）|edit（只修改） -->
            <form id="form1" runat="server">
            <!-- 必须有 < form > html标记，且待操作form字段被包含在里面-->
            <div>
                <label>
                    标题：
                </label>
                <input type="text" class="text" maxlength="50" name="Title" value="<% =read.Title %>" />
                <!-- name 属性是必须的，Title为占位符 ，id属性方便后面dom操作-->
            </div>
            <div>
                <label>
                    作者：
                </label>
                <input type="text" class="text" maxlength="20" name="Author" value="<% =read.Author %>" />
            </div>
            <div>
                <label>
                    日期：
                </label>
                <input type="text" class="text" maxlength="50" name="Time" value="<% =read.Time %>" />
            </div>
            <div>
                <label>
                    点击：
                </label>
                <input type="text" class="text" maxlength="10" name="View" value="<% =read.View %>" />
            </div>
            <div style="position: relative; height: 240px;">
                <label style="position: absolute; top: 120px;">
                    内容：
                </label>
                <textarea style="position: absolute; left: 43px;" name="Content" cols="40"
                    rows="10"><% =read.Content %></textarea>
            </div>
            <div style="clear: both; margin-left: 50px;">
                <input type="button" class="button" value="保存" onclick="_tagcall.form('FormTag_65559_9',true); return false;" /><!-- 最好不要用 type="submit"-->
                <!--_tagcall.form('',true); return false;为占位符，将生成 '_tagcall.form('form1',true); return false;'这里可以直接写后者-->
                <input type="reset" class="button" value="重置" />
                <input type="button" class="button" value="返回" onclick="location.href='jsonTag.aspx?backpage=<% = Request.QueryString["page"] %>'" />
            </div>
            <div style="margin-left: 70px;">
                <span id="result"></span>
            </div>
            
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
            <!-- 总结，form标签还是比较适用于只提交数据的场合（如留言板）-->
            <!--因为修改时，read读取数据时只适用于绑定<input type='text' /><taxtarea></taxtarea>-->
            <!--其他表单元素就不好绑定，难免需要用js操作-->
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
        if ('<% = Request.QueryString["id"] %>' == '') {
            alert('此页面需要有url参数 id !');
            location.href = 'jsonTag.aspx';
        }

        function CallbackSuccess(resultstr) {
            var obj = eval('(' + resultstr + ')');
            if (obj.type) {
                if (obj.type == "formcall") {//json标签请求的返回
                    if (obj.result.code == 0) {
                        $('#result').html('保存成功！');
                        setTimeout(function () {
                            $('#result').html('');
                        }, 3000);
                    }
                    else if (obj.result.code == 1) {
                        $('#result').html('不能为空！');
                        $('#' + obj.result.dom).focus();
                        setTimeout(function () {
                            $('#result').html('');
                        }, 3000);
                    }
                    else if (obj.result.code == 2) {
                        $('#result').html(obj.result.msg);
                        var dom = document.getElementsByName(obj.result.dom); //尝试以 name 查找文档元素
                        if (dom) {
                            $(dom).focus();
                        }
                    }
                    else if (obj.result.code == 3) {
                        $('#result').html(obj.result.msg);
                    }
                    else {
                        $('#result').html(obj.result.msg);
                    }
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
</body>
</html>
