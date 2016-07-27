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
        public static string GetCallBackJs()
        {
            string script = @"/* ajax 脚本支持 Created by VowsTag */
var _tagcall = {
    //发起请求. arg以'key1=value1&key2=value2..'的形式组成键值对,服务端重写public override CallbackResult DoCallback()以处理请求。
    //用方法:this.CallValue(key)可获取value1',context参数可省略.(详细请了解asp.net的'callback'机制)
    $: function(arg, async, context) {
        //请求发起前预处理方法，一般显示一个提示
        if (typeof(BeforCallback) == 'function')
        {
            BeforCallback();
        }
        WebForm_DoCallback('__Page', arg, this.success, this.error, context, async == undefined ? true : async == true);
    },
    success: function(result, context)//异步发起请求成功
    {
        //看是否有处理的方法
        if (typeof(CallbackSuccess) == 'function')
        {
            CallbackSuccess(result, context);
        }
        else
        {
            alert('服务端返回：\r\n' + result + '\r\n请实现js方法CallbackSuccess(result)以处理返回结果。');
        }
    },
    error: function(error, context)//异步发起请求错误
    {
        if (typeof(window.console) != undefined)
        {
            window.console.debug('出错了！--' + error);
        }
        //看是否有处理的方法
        if (typeof(CallbackError) == 'function')
        {
            CallbackError(error, context);
        }
        else
        {
            alert('请求出现错误！服务端返回：\r\n' + error + '\r\n请实现js方法CallbackError(result)以处理返回结果。');
        }
    },
    formqstr: '',
    form: function(formname, async)//form标签发起请求
    {
        var elemArray = theForm.elements;
        this.formqstr = '';
        for (var i = 0; i < elemArray.length; i++)
        {
            var element = elemArray[i];
            var elemType = element.type.toUpperCase();
            var elemName = element.name;
            if (elemName)
            {
                if (elemType == 'TEXT' || elemType == 'TEXTAREA'
                        || elemType == 'PASSWORD' || elemType == 'FILE' || elemType == 'HIDDEN')
                {
                    this.getElemValue(elemName, element.value);
                }
                else if (elemType == 'CHECKBOX' && element.checked) {
                this.getElemValue(elemName, element.value ? element.value : 'On');
                }
                else if (elemType == 'RADIO' && element.checked) {
                        this.getElemValue(elemName, element.value);
                }
                else if (elemType.indexOf('SELECT') != -1)
                {
                    for (var j = 0; j < element.options.length; j++)
                    {
                        var option = element.options[j];
                        if (option.selected)
                        {
                            this.getElemValue(elemName, option.value ? option.value : option.text);
                        }
                    }
                }
            }
        }
        this.$('formname=' + formname + '&' + this.formqstr, async, formname);
    },
    getElemValue: function(name, value)
    {
        if (name == '__VIEWSTATE' || name == '__VIEWSTATEGENERATOR' ||
                name == '__EVENTTARGET' || name == '__EVENTARGUMENT')
        {
            return;
        }
        this.formqstr += (this.formqstr.length > 0 ? '&' : '') + name + '=' + value;
    },
    json: function(jsonname, otenparams, async)//json标签发起请求
    {
        this.$('jsonname=' + jsonname + '&' + (otenparams || ''), async, jsonname);
    }
};
_tagcall.$json = _tagcall.json;//   _tagcall.$json 将弃用
var _tagcallback = _tagcall;//兼容旧版";
            return script;
        }
    }
}
