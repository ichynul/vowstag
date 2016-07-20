#region  The MIT License (MIT)
/*
The MIT License (MIT)

Copyright (c) 2015 ichynul

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
using System.Text;
namespace Tag.Vows.Tool
{
    class JsMaker
    {
        public static StringBuilder GetCallBackJs()
        {
            StringBuilder script = new StringBuilder(600);
            script.Append("        /* ajax 脚本支持 Created by VowsTag */\r\n");
            script.Append("        var _tagcall = {\r\n");
            script.Append("            //发起请求. arg以'key1=value1&key2=value2..'的形式组成键值对,服务端重写public override CallbackResult DoCallback()以处理请求。\r\n");
            script.Append("            //用方法:this.CallValue(\"key1\")可获取value1',context参数可省略.(详细请了解asp.net的'callback'机制)\r\n");
            script.Append("            $: function (arg ,async ,context)\r\n");
            script.Append("            {\r\n");
            script.Append("                //请求发起前预处理方法，一般显示一个提示\r\n");
            script.Append("                if (typeof(BeforCallback) == 'function' )\r\n");
            script.Append("                {\r\n");
            script.Append("                    BeforCallback();\r\n");
            script.Append("                }\r\n");
            script.Append("                WebForm_DoCallback('__Page', arg, this.success, this.error, context, async == true);\r\n"); ;
            script.Append("            },\r\n");
            script.Append("            success: function (result ,context)//异步发起请求成功\r\n");
            script.Append("            {\r\n");
            script.Append("                //看是否有处理的方法\r\n");
            script.Append("                if (typeof(CallbackSuccess) == 'function' )\r\n");
            script.Append("               {\r\n");
            script.Append("                    CallbackSuccess(result ,context);\r\n");
            script.Append("                }\r\n");
            script.Append("                else\r\n");
            script.Append("                {\r\n");
            script.Append("                    alert('服务端返回：\\r\\n' + result+'\\r\\n请实现js方法CallbackSuccess(result)以处理返回结果。');\r\n");
            script.Append("                }\r\n");
            script.Append("            },\r\n");
            script.Append("            error: function (error ,context)//异步发起请求错误\r\n");
            script.Append("            {\r\n");
            script.Append("                if (typeof(window.console) != undefined)\r\n");
            script.Append("                {\r\n");
            script.Append("                    window.console.debug( '出错了！--' + error );\r\n");
            script.Append("                }\r\n");
            script.Append("                //看是否有处理的方法\r\n");
            script.Append("                if (typeof(CallbackError) == 'function' )\r\n");
            script.Append("                {\r\n");
            script.Append("                    CallbackError(error ,context);\r\n");
            script.Append("                }\r\n");
            script.Append("                else\r\n");
            script.Append("                {\r\n");
            script.Append("                    alert('请求出现错误！服务端返回：\\r\\n' + error+'\\r\\n请实现js方法CallbackError(result)以处理返回结果。');\r\n");
            script.Append("                }\r\n");
            script.Append("            },\r\n");
            script.Append("            formqstr: '',\r\n");
            script.Append("            form: function (formname ,async)//form标签发起请求\r\n");
            script.Append("            {\r\n");
            script.Append("                var elemArray = theForm.elements;\r\n");
            script.Append("                this.formqstr ='';\r\n");
            script.Append("                for (var i = 0; i < elemArray.length; i++) {\r\n");
            script.Append("                    var element = elemArray[i];\r\n");
            script.Append("                    var elemType = element.type.toUpperCase();\r\n");
            script.Append("                    var elemName = element.name;\r\n");
            script.Append("                    if (elemName) {\r\n");
            script.Append("                        if ( elemType == 'TEXT' || elemType == 'TEXTAREA'\r\n");
            script.Append("                             || elemType == 'PASSWORD' || elemType == 'FILE' || elemType == 'HIDDEN')\r\n");
            script.Append("                        {\r\n");
            script.Append("                            this.getElemValue(elemName, element.value);\r\n");
            script.Append("                        }\r\n");
            script.Append("                        else if (elemType == 'CHECKBOX' && element.checked)\r\n");
            script.Append("                        {\r\n");
            script.Append("                            this.getElemValue(elemName, element.value ? element.value : 'On');\r\n");
            script.Append("                        }\r\n");
            script.Append("                        else if (elemType == 'RADIO' && element.checked)\r\n");
            script.Append("                        {\r\n");
            script.Append("                            this.getElemValue(elemName, element.value);\r\n");
            script.Append("                        }\r\n");
            script.Append("                        else if (elemType.indexOf('SELECT') != -1)\r\n");
            script.Append("                        {\r\n");
            script.Append("                            for (var j = 0; j < element.options.length; j++) {\r\n");
            script.Append("                                var option = element.options[j];\r\n");
            script.Append("                                if (option.selected)\r\n");
            script.Append("                                {\r\n");
            script.Append("                                    this.getElemValue(elemName, option.value ? option.value : option.text); }\r\n");
            script.Append("                                }\r\n");
            script.Append("                           }\r\n");
            script.Append("                       }\r\n");
            script.Append("                  }\r\n");
            script.Append("                this.$('formname=' + formname + '&' + this.formqstr ,async ,formname);\r\n");
            script.Append("            },\r\n");
            script.Append("            getElemValue: function(name ,value) {\r\n");
            script.Append("                if (name == '__VIEWSTATE' || name == '__VIEWSTATEGENERATOR'||\r\n");
            script.Append("                        name == '__EVENTTARGET' || name =='__EVENTARGUMENT')\r\n");
            script.Append("                {\r\n");
            script.Append("                    return;\r\n");
            script.Append("                }\r\n");
            script.Append("                this.formqstr += (this.formqstr.length > 0 ? '&' : '') + name + '=' + value;\r\n");
            script.Append("            },\r\n");
            script.Append("            json: function (jsonname ,otenparams ,async)//json标签发起请求\r\n");
            script.Append("            {\r\n");
            script.Append("                this.$('jsonname=' + jsonname + '&' + (otenparams || '') ,async ,jsonname);\r\n");
            script.Append("            }\r\n");
            script.Append("        };\r\n");
            script.Append("        _tagcall.$json =_tagcall.json;//   _tagcall.$json 将弃用\r\n");
            script.Append("        var _tagcallback = _tagcall;//兼容旧版\r\n");
            return script;
        }
    }
}
