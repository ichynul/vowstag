
**标签实例：**
1.1 列表-不分页
```
{#list = article? classid=url.id &orderby=istop,time &desc=true &take=10 & skip =3 }
    <a href='read.aspx?id={#item.id}'>{#item.title}</a> 
    <span>发表时间{#TimeFormat(item.DTime,"MM月dd日")}</span>
    <p>{# SubString(item.desc,50)}</p>
{#list}
```
1.2 列表-分页
```
{#list = article? classid >3 &id<1000 &orderby=tiem &desc=true &pzgesize=10 <!--# 分页时需要'pagesize'参数,不分页就用'take'这是注释，解析是略过-->}
    <img src='{#item.logo}'/>
    <a href='read.aspx?id={#item.id}'>{#item.title}</a>
{#list}
{#pager?type=cs}<!--分页标签-->
```
1.3 字符串包含(搜索)
```
{#list= article ? title % request.kwd &take=10 &item=search &emptytext=暂无相关记录 /}
<!--item参数引入另外的一个单独的文作为样式：(item/search.html)，以 /} 结尾-->
<!--%代表包含,不包含则为!%。-->
<!--request.xxx表示url中的参数xxx的值，类似的session.xxx、cookie.xxx、cookie.xx.yy分别为取session或cookie值-->
<!--request.xxx、req.xxx、url.xxx 三者等同-->
<!--emptytext=msg 指定记录为空时的简单提示信息，不能包含html-->
```
1.4 列表:被包含于
```
{#list= article ? id # [1,3,5,7,9,11] &title !# [hello,nihao] &item=newslist &skip=4 &take=10 /}
<!--#代表被包含于，后面接一个[]包围的数组,不被包含于则为!#。-->
<!--skip=4 跳过前4条-->
```
1.5 复杂逻辑支持
```
{#list= article ? classid >3 & ( none = req.id | id= req.id ) &item=newslist &skip=4 &take=10 /}
<!--#代表被包含于，后面接一个[]包围的数组,不被包含于则为!#。-->
<!--skip=4 跳过前4条-->
```
2.1 内容-有范围限制
```
{#read = article ? id=req.id /} <!--全局read以 /} 结尾 ，每个页面最多只能有一个全局read-->
    <h3>{#read.title}</h3> <!--{read.xxx}代表某个字段'xxx'-->
    <div>{#read.content}</div>
    {#click(“article”,10) }<!--后台方法,点击计数-->
{#read}

{#read.content} <!--超出作用范围-->

```
2.2 内容-全局
```
<span>{#read.title}</span> <!--全局的，字段值可以在任何地方引用，甚至在 reqd 标签前,一般用在页面title里面-->
{#read = article? id = request.id /} <!--读取指url参数指定的文章，该标签为全局的，以 /}结尾 ,每个页面最多有一个全局的read-->
{#click(“article”,request.id) }<!--后台方法，点击计数-->
<h3>{#read.title}</h3> <!--字段值，可写在任意位置-->
<div><span>作者:{reqd.author}</span><span>日期:{reqd.date}</span><span>点击:{reqd.view}</span></div>
<div>{#read.content}</div>
```

3.1：异步json
<!--json标签需在js脚本中使用-->
```
<script>
    var jsonname="{# json=article ? classid=call.type & title % request.kwd  &orderby=tiem &desc=true &pagesize=10 &fields=id,title,author,date,view /}"; 
    //读取栏目编号为 type ,标题中包含url参数kwd的文章,按时间倒序排列 ,fields 参数限制需要返回的字段，多个用,分割，无则返回全部自带
    //json 跟 list 差不多 就是多了fields参数，另外json不能用take参数
    var page = 1, pagesize;
    var pages, listsize=9999999;
    var type = 0;
    var over = false;
    function loadMore() {
        _tagcall.json(jsonname, "page=" + page + '&type=' + 3 ); //'type=3 => call.type'
    }
    
    //标签 callback请求成功处理
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
                            //处理json数组数据
                        }
                        else {
                            if (obj.result.page == 1) {
                                alert('暂无记录');
                            }
                            else {
                                alert('没有更多了');
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
</script>
```
**标签关键字 ：**

10.2 查询用关键字(标签：list/read/json/form):
skip 数值  跳过前几条数据
take 数值  获取前几条数据 （仅list）
item 字符串  指定外部样式 （仅list）
desc true/false  排序倒序，默认true 
orderby 字符串  排序字段，多个以,分割
pageszie 数值  分页大小
emptytext 字符串   数据为空时简单提示信息 （仅list）
fields 字符串  指定需要返回的字段 （仅json）
（null、empty、none）用于判断url参数是否存在、是否为空、前两者任意一种 如 null=url.type

10.2 分页用关键字(标签：pager):
type js/cs 分页类型 
edge 两端显示数量 a e
next 上一页 文本
prev 下一页 文本
num_display 中间显示显示页数 c
nextprev_show true/false 显示上一页、下一页 ,默认true
allways_show true/false 是否一直显示分页，默认false 即当只有一页时不显示分页。
ellipse 省略号.. b d
dom html元素 默认 div
domid html元素id 默认 pager
domclass html元素class 默认 mypager
上一页  1 2 3...7 8 9 10 11 12 13 ... 99 100 101 下一页
        ----a--b---------c----------d------e---

10.3 cmd指令 (标签：cmd):
callback 开启页面calbackjs脚本支持 当页面中有 json\form 标签是会自动开启，
validaterequest <% pgae 头部参数。默认为 true
enableviewstate <% pgae 头部参数。默认为 false
using 引用命名空间，多个用,分割

10.4 特殊关键字
request.xxx、req.xxx、url.xxx 三者等同，指 url链接中的参数 如 url.id ====> somepage.aspx?id=3
session.xxx、cookie.xxx、cookie.xx.yy分别为取session或cookie值
以上参数可以在查询标签内部使用，也可作为字符输出；
如 
``` 
标签查询 {#list=article?id》url.id &classid=session.cid... /}  
<span>{#cookie.uid}</span>
<script>
    var id='{#url.id}'; 
    alert(id); 
</script>
```
{#pgae} 当前页码
