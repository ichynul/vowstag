ʹ�ã�
 ```
//1.�������Tag.Vows.dll
    //2.����
     TagConfig __config= new mPaths();
    __config.input = "~/temple/"; /*ģ���ļ���Ϊ��Ŀ¼��/temple/�ļ���*/
    __config.output = "~/www/"; /*�����ļ���Ϊ��Ŀ¼��/www/�ļ���*/
    __config.db = new Entities(); /*����һ�� ObjectContext �� DbSet ����ʵ��*/
    __config.protected_tables = "Manages|Log";  /*�����ܱ����ı�*/
    __config.current_pairs = new string[2]{"{#","}"};/*���ñ�ǩ���ҷ���
    __config.creatScriptForAllPages = true;//Ϊ����ҳ������callback֧�֣�
                                        //������ÿ��ҳ����ʹ��{cmd?callback=true/}
    __config.defaultBase = "web.hello.pagebase";// ָ������ҳ�洦����Ĭ�ϼ̳е��ࣻ
                                              //Ĭ��Ϊ��Tag.Vows.TagPage��
                            //�����ھ���ҳ��ʹ��{cmd?base=web.hello.pagebase2 /}����ָ��
    //3.����ҳ�� /temple/page/test.html
    string msg = __config.MakePage("test");
    Response.Write(msg);
```   