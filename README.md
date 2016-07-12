﻿
**标签实例：**
1.1列表:不分页
```
{#list=article?classid=3 &orderby=tiem &desc=true &take=10} 
<!--读取栏目编号为3的文章,按时间倒序排列,获取结果的前10条记录-->
    <a href='read.aspx?id={#item.id}'>{#item.title}</a> 
    <!--{item.xxx}代表某个字段'xxx'-->
    <span>发表时间{#TimeFormat(item.DTime,"MM月dd日")}</span>
    <!--{TimeFormat(xxxx，“”)}格式化日期时间-->
    <p>{# SubString(item.desc,50)}</p>
    <!--{SubString(xxxx)}为一个内置截取字符串的方法,此次截取文字描述的50个字-->
{#list}<!--标签结尾-->
```

1.2列表:分页
```
{#list=article? classid >3 &id<1000 &orderby=tiem &desc=true &pzgesize=10} 
<!--读取栏目编号大于3且id小于1000的文章,按时间倒序排列,分页大小为10-->
    <img src='{#item.logo}'/>
    <a href='read.aspx?id={#item.id}'>{#item.title}</a>
{#list}
{#pager?type=cs}<!--分页标签-->
```

1.3列表:字符串包含(搜索)
```
{#list=article?title%request.kwd&take=10&item=search&emptytext=暂无相关记录 /} 
<!--省略了orderby和desc，按默认-->
<!--此标签为list标签的另一种形式，把样式放在另外的一个单独的文件里面（item=search）,没有{#list}相匹配，而是以/}结尾-->
<!--%代表包含,不包含则为!%。-->
<!--request.xxx表示url中的参数xxx的值，类似的session.xxx、cookie.xxx、cookie.xx.yy分别为取session或cookie值-->
<!--emptytext=msg指定记录为空时的提示信息-->
```
1.4列表:被包含于
```
{#list=article?id#[1,3,5,7,9,11] &title!#[hello,nihao] &take=10 &item=newslist &skip=4 /}
<!--#代表被包含于，后面接一个[]包围的数组,不被包含于则为!#。-->
<!--skip=4 跳过前4条-->
```

2.1：内容
```
{#read=article?id=10} <!--读取指定id的文章-->
    <h3>{#read.title}</h3> <!--{read.xxx}代表某个字段'xxx'-->
    <div>{#read.content}</div>
    {#click(“article”,10) }<!--后台方法,点击计数-->
{#read}<!--标签结尾-->
```

2.1：内容url参数
```
{#read=article?id=request.id /} <!--读取指url参数指定的文章，该标签为全局的，以 /}结尾-->
<h3>{#read.title}</h3> <!--字段值，可写在任意位置-->
<div>{#read.content}</div>
{#click(“article”,request.id) }<!--后台方法，点击计数-->
```

3.1：异步json
```
<a href="javascript:;" onclick="prevPage()">上一页</a><a href="javascript:;" onclick="nextPage()">下一页</a>
<!--json标签需在js脚本中使用-->
<script>
     var page = 1, pagesize;
     var pages, listsize=9999999;
     var type = 0;
     var over = false;
    var jsonname="{#json=article?classid=3&orderby=tiem&desc=true}"; //读取栏目编号为3的文章,按时间倒序排列
 
    function loadMore() {
        _tagcall.$json(jsonname, "page=" + page);
    }
    
    //下一页
    function nextPage()
    {
        page+=1;
        if(page>listsize)
        {
            return;
        }
        loadMore();
    }
    
    //上一页
    function prevPage()
    {
        page-=1;
        if(page<1)
        {
            return;
        }
        loadMore();
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
                            //处理json数据
                            /*var tpl = $('#list-template').text();
                            var tempFn = doT.template(tpl);
                            $("#thelist").html(tempFn(obj.result.data));*/
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
```
** 使用：**
    //1.添加引用Tag.Vows.dll
    //2.配置
     TagConfig __config= new mPaths();
    __config.input = "~/temple/"; /*模板文件夹为根目录的/temple/文件夹*/
    __config.output = "~/www/"; /*生成文件夹为根目录的/www/文件夹*/
    __config.db = new Entities(); /*设置一个 ObjectContext 或 DbSet 的子实例*/
    __config.protected_tables = "Manages|Log";  /*设置受保护的表*/
    __config.current_pairs = new string[2]{"{#","}"};/*配置标签左右符号 {#....../}
    __config.creatScriptForAllPages = true;//为所有页面生成callback支持；
                                        //无需在每个页面中使用{cmd?callback=true/}
    __config.defaultBase = "web.hello.pagebase";// 指定所有页面处理类默认继承的类；
                                              //默认为‘Tag.Vows.TagPage’
                            //可在在具体页面使用{cmd?base=web.hello.pagebase2 /}另行指定
    //3.生成页面 /temple/page/test.html
    string msg = __config.MakePage("test");
    Response.Write(msg);
```