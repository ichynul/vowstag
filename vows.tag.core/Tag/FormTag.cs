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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tag.Vows.Bean;
using Tag.Vows.Data;
using Tag.Vows.Interface;
using Tag.Vows.Tool;

namespace Tag.Vows.Tag
{
    class FormTag : BaseTag, ITableUseable, ICallBackAble
    {
        private string BaseParams;
        private List<TagWhere> baseWheres;
        private Method CallBack;
        public string DataName;
        private string
            allowempty,
            action = "both";
        List<string> fields = new List<string>();

        public List<FromVar> Vars = new List<FromVar>();
        private object model;
        //
        string type = "";
        string name = "";
        bool isNullAble = false;
        string newName = "";
        //
        public FormTag(string mtext, string mOrigin, int Deep, TagConfig config, int no_)
            : base(mtext, mOrigin, Deep, config, no_)
        {
        }

        public void AddField(FieldTag field)
        {
            if (model == null)
            {
                return;
            }
            name = field.getParamAt(1);
            type = DataHelper.GetType(model, name, out isNullAble, out newName);
            if (!fields.Contains(newName))
            {
                Vars.Add(new FromVar(newName, name, type, isNullAble));
                fields.Add(newName);
            }
        }

        public void SetMethod(MethodTag method)
        {
            method.forname = this.GetTagName();
        }
        protected override string GetCodeForAspx()
        {
            return string.Format("<!-- {0} -->", this.TagName);
        }

        protected override void Discover()
        {
            this.DataName = this.Config.tagregex.getDataName(this.Text);
            this.BaseParams = this.Config.tagregex.getBaseParams(this.Text);
            if (string.IsNullOrEmpty(this.BaseParams))
            {
                this.BaseParams = "desc = true";
            }
            model = TempleHelper.getTempleHelper(this.Config).GetModObj(DataName);
            if (!string.IsNullOrEmpty(BaseParams))
            {
                baseWheres = TempleHelper.getTempleHelper(this.Config).Linq_queryParams(model, BaseParams);
                TagWhere add = baseWheres.FirstOrDefault(x => x.FiledName == "action");
                if (add != null)
                {
                    this.action = add.VarName;
                }
            }
            else
            {
                BaseParams = "";
            }
        }

        public override string ToTagString()
        {
            return "【全局名称" + this.GetTagName() + ",标签类型：form，数据源名称：" + this.DataName + "，数据参数：" + this.BaseParams + "】<br />";
        }

        public string GetCode()
        {
            if (model == null)
            {
                return string.Format("{0}//无效的表名称 {1}", Method.getSpaces(2), DataName);
            }
            if (!CheckDataUseable())
            {
                return string.Format("{0}//{1}", Method.getSpaces(2), this.TabledisAbledMsg());
            }
            string modType = model.GetType().Name;
            string dataName = "_" + modType.ToLower();
            StringBuilder linq = new StringBuilder(Method.getSpaces(2) + "/*" + BaseParams + "*/\r\n");
            linq.AppendFormat("{0}string formname = this.CallValue(\"formname\");\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}if (formname != \"{1}\")\r\n", Method.getSpaces(2), this.GetTagName());
            linq.Append(Method.getSpaces(2) + "{\r\n");
            linq.Append(Method.getSpaces(3) + "return null;\r\n");
            linq.Append(Method.getSpaces(2) + "}\r\n");
            string vname = "";
            //
            linq.AppendFormat("{0}dynamic error = new ExpandoObject();\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}error.formname = formname;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}CallBackResult call = new CallBackResult(error) ;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}call.type = \"formcall\";\r\n", Method.getSpaces(2));
            if (action != "add")
            {
                linq.AppendFormat("{0}{1} {2} = null ;\r\n", Method.getSpaces(2), modType, dataName);
                bool first = true;
                List<string> orderbylist = new List<string>();
                bool desc = true;
                foreach (var w in baseWheres)
                {
                    if (w.FiledName == "orderby")
                    {
                        orderbylist.AddRange(w.VarName.Split(',').Where(x => x.Length > 0));
                        continue;
                    }
                    else if (w.FiledName == "desc")
                    {
                        desc = w.VarName.ToLower() == "true";
                        continue;
                    }
                    else if (w.FiledName == "allowempty")
                    {
                        allowempty = w.VarName.ToLower();
                        continue;
                    }
                    else if (w.FiledName == "action")
                    {
                        continue;
                    }
                    if (first)
                    {
                        linq.Append(TempleHelper.getTempleHelper(this.Config).GetWhereParams(model, baseWheres, BaseParams));
                        linq.AppendFormat("\r\n{0}{1} = {2}.FirstOrDefault( b=>\r\n {3}", Method.getSpaces(2),
                            dataName, Config.GetingTableStr("list", modType), Method.getSpaces(4));
                        w.LogicSymb = Method.getSpaces(1);
                        first = false;
                    }
                    if (w.FiledName == "null")
                    {
                        linq.AppendFormat("{0}{1}{2} {3}\r\n{4}", w.LogicSymb, w.FieldLeft, w.VarName, w.VarRight, Method.getSpaces(5));
                    }
                    else if (w.Compare == "%")
                    {
                        linq.AppendFormat("{0}{1}b.{2}.Contains({3}){4}\r\n{5}", w.LogicSymb, w.FieldLeft, w.FiledName, w.VarName, w.VarRight, Method.getSpaces(5));

                    }
                    else if (w.Compare == "!%")
                    {
                        linq.AppendFormat("{0}{1}!b.{2}.Contains({3}){4}\r\n{5}", w.LogicSymb, w.FieldLeft, w.FiledName, w.VarName, w.VarRight, Method.getSpaces(5));
                    }
                    else
                    {
                        linq.AppendFormat("{0}{1}b.{2} {3} {4}{5}\r\n{6}", w.LogicSymb, w.FieldLeft, w.FiledName, w.Compare, w.VarName, w.VarRight, Method.getSpaces(5));
                    }
                }
                if (!first)
                {
                    linq.Append(");\r\n");
                    if (orderbylist.Count() > 0)
                    {
                        linq.AppendFormat(".{0}(c=>c.{1})", desc ? "OrderByDescending" : "OrderBy", DataHelper.GetPropertyName(model, orderbylist[0]));
                        if (orderbylist.Count() > 1)
                        {
                            for (int i = 1; i < orderbylist.Count(); i += 1)
                            {
                                linq.AppendFormat("\r\n{0}.{1}(c=>c.{2})", Method.getSpaces(5), desc ? "ThenByDescending" : "ThenBy",
                                    DataHelper.GetPropertyName(model, orderbylist[i]));
                            }
                        }
                    }
                }
                //

                linq.AppendFormat("{0}if ({1} == null)\r\n", Method.getSpaces(2), dataName);
                linq.Append(Method.getSpaces(2) + "{\r\n");
                if (action == "edit")
                {
                    linq.AppendFormat("{0}/*编辑模式（action = edit），未找到记录则返回*/;\r\n", Method.getSpaces(3));
                    linq.AppendFormat("{0}error.code = 3;\r\n", Method.getSpaces(3));
                    linq.AppendFormat("{0}error.msg = \"操作失败：not found！\";\r\n", Method.getSpaces(3));
                    linq.AppendFormat("{0}error.dom = \"\";\r\n", Method.getSpaces(3));
                    linq.AppendFormat("{0}return call;\r\n", Method.getSpaces(3));
                }
                else
                {
                    linq.AppendFormat("{0}/*添加&编辑模式（action = both），未找到记录则创建*/;\r\n", Method.getSpaces(3));
                    linq.AppendFormat("{0}{1} = new {2}();\r\n", Method.getSpaces(3), dataName, modType);
                    linq.AppendFormat("{0}Db_Context.{1}.{2}({3});\r\n", Method.getSpaces(3), modType, TempleHelper.getTempleHelper(this.Config).GetAddMethod(modType), dataName);
                }
                linq.Append(Method.getSpaces(2) + "}\r\n");

            }
            else
            {
                linq.AppendFormat("{0}/*添加模式（action = both），新建记录*/;\r\n", Method.getSpaces(3));
                linq.AppendFormat("{0}{1} {2} = new {1}();\r\n", Method.getSpaces(2), modType, dataName);
                linq.AppendFormat("{0}Db_Context.{1}.{2}({3});\r\n", Method.getSpaces(2), modType, TempleHelper.getTempleHelper(this.Config).GetAddMethod(modType), dataName);
            }
            FromVar v = null;
            vname = "";
            for (int i = 0; i < Vars.Count; i += 1)
            {
                v = Vars[i];
                vname = string.Format("_{0}", v.Name);
                linq.AppendFormat("{0}string {1} = this.CallValue(\"{2}\");\r\n", Method.getSpaces(2), vname, v.Name);
                if (v.Type == "string")
                {
                    if (string.IsNullOrEmpty(allowempty) || !allowempty.Contains(v.Name.ToLower()))
                    {
                        linq.AppendFormat("{0}if (string.IsNullOrEmpty({1}))\r\n", Method.getSpaces(2), vname);
                        linq.Append(Method.getSpaces(2) + "{\r\n");
                        linq.AppendFormat("{0}error.code = 1;\r\n", Method.getSpaces(3));
                        linq.AppendFormat("{0}error.msg = \"不能为空！\";\r\n", Method.getSpaces(3));
                        linq.AppendFormat("{0}error.dom = \"{1}\";\r\n", Method.getSpaces(3), v.OName);
                        linq.AppendFormat("{0}return call;\r\n", Method.getSpaces(3));
                        linq.Append(Method.getSpaces(2) + "}\r\n");
                    }
                    linq.AppendFormat("{0}{1}.{2} = {3};\r\n", Method.getSpaces(2), dataName, v.Name, vname);
                }
                else if (v.Type == "bool")
                {
                    linq.AppendFormat("{0}{1}.{2} = {3} != null && ({3}.ToLower() == \"on\" || {3}.ToLower() == \"true\" || {3} == \"1\") ;\r\n", Method.getSpaces(2), dataName, v.Name, vname);
                }
                else
                {
                    if (v.Type == "DateTime")
                    {
                        linq.AppendFormat("{0}{1} _{2} = {1}.Now;\r\n", Method.getSpaces(2), v.Type, vname);

                    }
                    else if ("int|long|float|double|decimal".Contains(v.Type))
                    {
                        linq.AppendFormat("{0}{1} _{2} = 0 ;\r\n", Method.getSpaces(2), v.Type, vname);
                    }
                    else
                    {
                        linq.AppendFormat("{0}// 未处理的类型 {1}", Method.getSpaces(2), v.Type);
                        continue;
                    }
                    linq.AppendFormat("{0}if ({1}.TryParse({2},out _{2}))\r\n", Method.getSpaces(2), v.Type, vname);
                    linq.Append(Method.getSpaces(2) + "{\r\n");
                    linq.AppendFormat("{0}{1}.{2} = _{3} ;\r\n", Method.getSpaces(3), dataName, v.Name, vname);
                    linq.Append(Method.getSpaces(2) + "}\r\n");
                    if (string.IsNullOrEmpty(allowempty) || !allowempty.Contains(v.Name.ToLower()))
                    {
                        linq.Append(Method.getSpaces(2) + "else\r\n");
                        linq.Append(Method.getSpaces(2) + "{\r\n");
                        linq.AppendFormat("{0}if (string.IsNullOrEmpty({1}))\r\n", Method.getSpaces(3), vname);
                        linq.Append(Method.getSpaces(3) + "{\r\n");
                        linq.AppendFormat("{0}error.code = 1;\r\n", Method.getSpaces(4));
                        linq.AppendFormat("{0}error.msg = \"不能为空！\";\r\n", Method.getSpaces(4));
                        linq.AppendFormat("{0}error.dom = \"{1}\";\r\n", Method.getSpaces(4), v.OName);
                        linq.AppendFormat("{0}return call;\r\n", Method.getSpaces(4));
                        linq.Append(Method.getSpaces(3) + "}\r\n");
                        linq.Append(Method.getSpaces(3) + "else\r\n");
                        linq.Append(Method.getSpaces(3) + "{\r\n");
                        linq.AppendFormat("{0}error.code = 2;\r\n", Method.getSpaces(4));
                        linq.AppendFormat("{0}error.msg = \"请输入正确的{1}\";\r\n", Method.getSpaces(4),
                            v.Type == "DateTime" ? "时间" : "int|long".Contains(v.Type) ? "整数" : "小数");
                        linq.AppendFormat("{0}error.dom = \"{1}\";\r\n", Method.getSpaces(4), v.OName);
                        linq.AppendFormat("{0}return call;\r\n", Method.getSpaces(4));
                        linq.Append(Method.getSpaces(3) + "}\r\n");
                        linq.Append(Method.getSpaces(2) + "}\r\n");
                    }
                }
            }
            //
            linq.AppendFormat("{0}//\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}Db_Context.SaveChanges();\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}error.code = 0;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}error.msg = \"操作成功！\";\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}error.dom = \"\";\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}return call;\r\n", Method.getSpaces(2));
            return linq.ToString();
        }

        public bool CheckDataUseable()
        {
            return this.Config.TableUseable(this.DataName);
        }

        public string TabledisAbledMsg()
        {
            return string.Format("数据表{0}已被设置为不可用，您无权操作该表！", this.DataName);
        }

        public Method GetCallMethod()
        {
            if (CallBack == null)
            {
                CallBack = new Method();
                CallBack.Name = "CallBack_" + this.GetTagName();
                CallBack.ReturnType = "CallbackResult";
                CallBack.InPageLoad = false;
                if (!CheckDataUseable())
                {
                    CallBack.Body.AppendFormat("{0}/*{1}*/\r\n", Method.getSpaces(2), this.TabledisAbledMsg());
                }
                else
                {
                    CallBack.Body.Append(this.GetCode());
                }
            }

            return CallBack;
        }
    }
}
