 <table cellpadding="0">
        <tr><td class="name">��飺</td><td>vowsTag��ǩʹ��˵��</td></tr>
</td></tr>
        <tr>
            <td class="name">�ļ��ṹ��</td>
            <td>
                <ul class="format" style=" ">
                    <li>ҳ���ļ�֧��.html��.htm</li>
                    <li>item/<span class="space"></span>���list��ǩ��item���ļ���</li>
                    <li>label/<span class="space"></span>���labelҳ����ļ���</li>
                    <li>page/<span class="space"></span>�����ҳ��ҳ����ļ���</li>
                    <li>static/<span class="space"></span>���staticҳ����ļ���</li>
                </ul>
            </td>
        </tr>
        <tr>
            <td class="name">������</td>
            <td>
                <ul class="format" style=" ">
                    <li>��ǩ��һ�Դ����Ű�Χ {...}��{.../}��{...}...{/...}</li>
                    <li>��ǩ�ڲ������������ո����</li>
                    <li>����ĳЩ�������ı�ǩ,����'&lt;br /&gt;' ����'&'��'&lt;hr /&gt;'����'|'</li>
                    <li>��ǩ���߼����Ƕ�·�ģ�'&'ʵ��Ч��Ϊ'&&'��'|'ʵ��Ч��Ϊ'||'</li>
                </ul>
            </td>
        </tr>
    </table>
```
** ʹ�ã�**
    //1.�������TagCore.dll
    //2.����
     TagConfig __config= new mPaths();
    __config.input = "~/temple/"; /*ģ���ļ���Ϊ��Ŀ¼��/temple/�ļ���*/
    __config.output = "~/www/"; /*�����ļ���Ϊ��Ŀ¼��/www/�ļ���*/
    __config.db = new Entities(); /*����һ�� ObjectContext �� DbSet ����ʵ��*/
    __config.protected_tables = "Manages|Log";  /*�����ܱ����ı�*/
    __config.current_pairs = new string[2]{"{#","}"};/*���ñ�ǩ���ҷ��� {#....../}
    __config.creatScriptForAllPages = true;//Ϊ����ҳ������callback֧�֣�
                                        //������ÿ��ҳ����ʹ��{cmd?callback=true/}
    __config.defaultBase = "web.hello.pagebase";// ָ������ҳ�洦����Ĭ�ϼ̳е��ࣻ
                                              //Ĭ��Ϊ��Tag.Vows.TagPage��
                            //�����ھ���ҳ��ʹ��{cmd?base=web.hello.pagebase2 /}����ָ��
    //3.����ҳ�� /temple/page/test.html
    string msg = __config.MakePage("test");
    Response.Write(msg);
```
**��ǩʵ����**
1.1�б�:����ҳ
```
{#list=article?classid=3&orderby=tiem&desc=true&take=10} <!--��ȡ��Ŀ���Ϊ3������,��ʱ�䵹������,��ȡ�����ǰ10����¼-->
    <a href='read.aspx?id={#item.id}'>{#item.title}</a> <!--{item.xxx}����ĳ���ֶ�'xxx'-->
    <p>{# SubString(item.desc,50)}</p><!--{SubString(xxxx)}Ϊһ�����ý�ȡ�ַ����ķ���,�˴ν�ȡ����������50����-->
{#list}<!--��ǩ��β-->
```

1.2�б�:��ҳ
```
{#list=article?classid>3&id<1000&orderby=tiem&desc=true&pzgesize=10} <!--��ȡ��Ŀ��Ŵ���3��idС��1000������,��ʱ�䵹������,��ҳ��СΪ10-->
    <img src='{#item.logo}'/>
    <a href='read.aspx?id={#item.id}'>{#item.title}</a>
{#list}
{#pager?type=cs}<!--��ҳ��ǩ-->
```

1.3�б�:�ַ�������(����)
```
{#list=article?title%request.kwd&take=10&item=search&emptytext=������ؼ�¼/} <!--ʡ����orderby��desc����Ĭ��-->
<!--�˱�ǩΪlist��ǩ����һ����ʽ������ʽ���������һ���������ļ����棨item=search��,û��{#list}��ƥ�䣬������/}��β-->
<!--%�������,��������Ϊ!%��-->
<!--request.xxx��ʾurl�еĲ���xxx��ֵ-->
<!--emptytext=msgָ����¼Ϊ��ʱ����ʾ��Ϣ-->
```

2.1������
```
{#read=article?id=10} <!--��ȡָ��id������-->
    <h3>{#read.title}</h3> <!--{read.xxx}����ĳ���ֶ�'xxx'-->
    <div>{#read.content}</div>
    {#{click()}}
{#read}<!--��ǩ��β-->
```

2.1������url����
```
    {#read=article?id=request.id /} <!--��ȡָurl����ָ�������£��ñ�ǩΪȫ�ֵģ��� /}��β-->
    <h3>{#read.title}</h3> <!--�ֶ�ֵ����д������λ��-->
    <div>{#read.content}</div>
    {#click(request.id)}<!--��̨�������������-->
```

3.1���첽json
```
<a href="javascript:;" onclick="prevPage()">��һҳ</a><a href="javascript:;" onclick="nextPage()">��һҳ</a>
<!--json��ǩ����js�ű���ʹ��-->
<script>
     var page = 1, pagesize;
     var pages, listsize=9999999;
     var type = 0;
     var over = false;
    var jsonname="{#json=article?classid=3&orderby=tiem&desc=true}"; //��ȡ��Ŀ���Ϊ3������,��ʱ�䵹������
 
    function loadMore() {
        _tagcall.$json(jsonname, "page=" + page);
    }
    
    //��һҳ
    function nextPage()
    {
        page+=1;
        if(page>listsize)
        {
            return;
        }
        loadMore();
    }
    
    //��һҳ
    function prevPage()
    {
        page-=1;
        if(page<1)
        {
            return;
        }
        loadMore();
    }
    
    //��ǩ callback����ɹ�����
    function CallbackSuccess(resultstr) {
            var obj = eval('(' + resultstr + ')');
            if (obj.type) {
                if (obj.type == "jsoncall") {//json��ǩ����ķ���
                    if (obj.result && obj.result.data) {
                        over = obj.result.over;
                        page = obj.result.page;
                        listsize = obj.result.listsize;
                        pagesize = obj.result.pagesize;
                        paging();
                        if (obj.result.data.length > 0) {
                            //����json����
                            /*var tpl = $('#list-template').text();
                            var tempFn = doT.template(tpl);
                            $("#thelist").html(tempFn(obj.result.data));*/
                        }
                        else {
                            if (obj.result.page == 1) {
                                alert('���޼�¼');
                            }
                            else {
                                alert('û�и�����');
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