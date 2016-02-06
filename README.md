使用：
 ```
//1.添加引用Tag.Vows.dll
    //2.配置
     TagConfig __config= new mPaths();
    __config.input = "~/temple/"; /*模板文件夹为根目录的/temple/文件夹*/
    __config.output = "~/www/"; /*生成文件夹为根目录的/www/文件夹*/
    __config.db = new Entities(); /*设置一个 ObjectContext 或 DbSet 的子实例*/
    __config.protected_tables = "Manages|Log";  /*设置受保护的表*/
    __config.current_pairs = new string[2]{"{#","}"};/*配置标签左右符号
    __config.creatScriptForAllPages = true;//为所有页面生成callback支持；
                                        //无需在每个页面中使用{cmd?callback=true/}
    __config.defaultBase = "web.hello.pagebase";// 指定所有页面处理类默认继承的类；
                                              //默认为‘Tag.Vows.TagPage’
                            //可在在具体页面使用{cmd?base=web.hello.pagebase2 /}另行指定
    //3.生成页面 /temple/page/test.html
    string msg = __config.MakePage("test");
    Response.Write(msg);
```   