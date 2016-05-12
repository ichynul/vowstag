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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Tag.Vows.Bean;
using Tag.Vows.Enum;
using Tag.Vows.Interface;
using Tag.Vows.Page;
using Tag.Vows.Tag;
using Tag.Vows.Tool;

namespace Tag.Vows.Data
{
    /// <summary>
    /// TempleHelper 的摘要说明
    /// 2015/07/10
    /// lianghaiyun
    /// </summary>
    /// 
    sealed class TempleHelper
    {
        private object UpperDataModel = null;
        private static TempleHelper Helper;
        private TempleHelper() { }
        private TagConfig Config;
        private static Dictionary<string, string> _dataTypes;
        private static Dictionary<string, string> dataTypes
        {
            get
            {
                if (_dataTypes == null)
                {
                    _dataTypes = new Dictionary<string, string>();
                    _dataTypes.Add("Byte", "byte");
                    _dataTypes.Add("Int16", "short");
                    _dataTypes.Add("Int32", "int");
                    _dataTypes.Add("Int64", "long");
                    _dataTypes.Add("String", "string");
                    _dataTypes.Add("DateTime", "DateTime");
                    _dataTypes.Add("Double", "double");
                    _dataTypes.Add("Float", "float");
                    _dataTypes.Add("Boolean", "bool");
                    _dataTypes.Add("Decimal", "decimal");
                }
                return _dataTypes;
            }
        }

        public static TempleHelper getTempleHelper(TagConfig config)
        {
            if (Helper == null)
            {
                Helper = new TempleHelper();
            }
            Helper.Config = config;
            return Helper;
        }

        #region GetWhereParams
        public StringBuilder GetWhereParams(object model, List<TagWhere> wheres, string baseParms)
        {
            StringBuilder sb = new StringBuilder();
            string passNames = "orderby|desc|pagesize|take|item|skip";
            string dataType = "";
            int i = 0;
            Match m = null;
            string itemOrRead = "";
            string filed = "";
            string vname = "";
            string ifNullvar = "";
            string ifTag = "";
            string query = "";
            string q = "";
            bool mNullAble = false;
            List<string> requestTests = new List<string>();
            List<string> requests = new List<string>();
            foreach (TagWhere w in wheres)
            {
                i += 1;
                if (passNames.Contains(w.FiledName))
                {
                    continue;
                }
                if (w.FiledName == "null" || w.FiledName == "empty" || w.FiledName == "none")
                {
                    if (!Regex.IsMatch(w.VarName, @"^request|call\.\w+$", RegexOptions.IgnoreCase))
                    {
                        sb.AppendFormat(
                            "{0}/* {1}有误！仅支持 null|empty|none  与request 或 call 参数进行比较，"
                            + "如 null判断url参数是否存在(==null)，empty 判断url参数存在但为空(==\"\")，none为null或empey其一(string.IsNullOrEmpty). */\r\n",
                            Method.getSpaces(2), w.VarName);
                    }
                    else if (w.Compare != "==" && w.Compare != "!=")
                    {
                        sb.AppendFormat("{0}/* 错误的操作 只能为 = 或 != */\r\n", Method.getSpaces(2), w.VarName);
                    }
                    else
                    {
                        query = this.Config.tagregex.RequestValue.IsMatch(w.VarName) ? "\"\" + Request.QueryString" : "\"\" + this.CallString";
                        vname = Regex.Replace(w.VarName, @"(?:request|call)\.", string.Empty, RegexOptions.IgnoreCase);
                        ifTag = string.Format("{0}{1}|", w.FiledName, vname);
                        if (w.FiledName == "null")
                        {
                            ifNullvar = string.Format("ifNull_{0}", w.VarName.Split('.')[1]);
                            w.VarName = ifNullvar;
                            if (requestTests.Contains(ifTag))
                            {
                                continue;
                            }
                            sb.AppendFormat("{0}bool {1} = null {2} {3}[\"{4}\"];\r\n",
                                Method.getSpaces(2), ifNullvar, w.Compare, query, vname);
                        }
                        else if (w.FiledName == "empty")
                        {
                            ifNullvar = string.Format("ifEmpty_{0}", w.VarName.Split('.')[1]);
                            w.VarName = ifNullvar;
                            if (requestTests.Contains(ifTag))
                            {
                                continue;
                            }
                            sb.AppendFormat("{0}bool {1} = \"\" {2} {3}[\"{4}\"];\r\n",
                                    Method.getSpaces(2), ifNullvar, w.Compare, query, vname);
                        }
                        else if (w.FiledName == "none")
                        {
                            ifNullvar = string.Format("ifNullOrEmpty_{0}", w.VarName.Split('.')[1]);
                            w.VarName = ifNullvar;
                            if (requestTests.Contains(ifTag))
                            {
                                continue;
                            }
                            sb.AppendFormat("{0}bool {1} =  {2}string.IsNullOrEmpty({3}[\"{4}\"]);\r\n",
                                    Method.getSpaces(2), ifNullvar, w.Compare == "==" ? null : "!", query, vname);
                        }
                        requestTests.Add(ifTag);
                    }
                    continue;
                }
                dataType = GetObjType(model, w.FiledName, out mNullAble, out filed);
                w.FiledName = filed;
                if (w.VarName.ToLower() == "null")
                {
                    if (dataType != "string" && mNullAble)
                    {
                        w.VarName = "null/*错误！该字段不是string或者NullAble类型*/";
                    }
                    continue;
                }
                if (Regex.IsMatch(w.VarName, @""".*?"""))
                {
                    if (dataType != "string")
                    {
                        w.VarName = string.Concat(w.VarName, "/*错误！该字段不是string类型*/");
                    }
                    continue;
                }
                if (w.Compare == "%" || w.Compare == "!%")
                {
                    if (dataType != "string")
                    {
                        w.Compare = w.Compare == "%" ? "==" : "!=";
                        sb.AppendFormat("{0}/*{1}.{2} 不是字符串，不能使用( %-包含 或 !%-不包含 ) 操作符，已替换为 = 或 != */\r\n",
                            Method.getSpaces(2), w.VarName.Contains("item") ? UpperDataModel : model, filed);
                    }
                }
                if (Regex.IsMatch(w.VarName, @"(?:read|item)\.\w+", RegexOptions.IgnoreCase))
                {
                    w.FiledName = GetPropertyName(model, w.FiledName);
                    if (w.VarName.Contains("-") || w.VarName.Contains("+") || w.VarName.Contains("*") || w.VarName.Contains("/"))
                    {
                        string MathSymb = w.VarName.Contains("-") ? "-" : w.VarName.Contains("+") ? "+" : w.VarName.Contains("*") ? "*" : "/";
                        m = new Regex(@"\d+(\.\d+)?").Match(w.VarName);
                        string number = m.Value;
                        itemOrRead = w.VarName.ToLower().Contains("item") ? "item" : "read";

                        dataType = GetObjType(UpperDataModel, Regex.Replace(
                            w.VarName, @"item|read|[\.\+\-\*/\d]", string.Empty, RegexOptions.IgnoreCase), out mNullAble, out filed);
                        vname = string.Format("{0}_{1}_{2}", dataType, filed, i);
                        if (mNullAble)
                        {
                            sb.AppendFormat("{0}{1} {2} = {3};\r\n", Method.getSpaces(2), dataType, vname,
                                    string.Format("{0} == null || !{0}.{1}.HasValue ? {2}.MinValue : {0}.{1}.Value {3}{4};\r\n",
                                            itemOrRead, filed, dataType, MathSymb, number));
                        }
                        else
                        {
                            sb.AppendFormat("{0}{1} {2} = {3} == null ? {1}.MinValue: {3}.{4} {5} {6};\r\n",
                                        Method.getSpaces(2), dataType, vname, itemOrRead, filed, MathSymb, number);
                        }
                    }
                    else
                    {
                        dataType = GetObjType(UpperDataModel, Regex.Replace(
                             w.VarName, @"item|read|\.", string.Empty, RegexOptions.IgnoreCase), out mNullAble, out filed);
                        vname = string.Format("{0}_{1}_{2}", dataType, filed, i);
                        itemOrRead = w.VarName.ToLower().Contains("item") ? "item" : "read";
                        if (dataType == "string")
                        {
                            sb.AppendFormat("{0}{1} {2} = {3 } == null ? \"!@#$%\": {3}.{4};\r\n",
                                            Method.getSpaces(2), dataType, vname, itemOrRead, filed);
                        }
                        else
                        {
                            if (mNullAble)
                            {
                                sb.AppendFormat("{0}{1} {2} = {3};\r\n", Method.getSpaces(2), dataType, vname,
                                       string.Format("{0} == null || !{0}.{1}.HasValue ? {2}.MinValue : {0}.{1}.Value;\r\n",
                                                itemOrRead, filed, dataType));
                            }
                            else
                            {
                                sb.AppendFormat("{0}{1} {2} = {3 } == null ? {1}.MinValue: {3}.{4};\r\n",
                                            Method.getSpaces(2), dataType, vname, itemOrRead, filed);
                            }
                        }
                    }
                    w.VarName = vname;
                }
                else if (Regex.IsMatch(w.VarName, @"(?:request|session|cookie|call)\.\w+", RegexOptions.IgnoreCase))
                {
                    dataType = GetObjType(model, w.FiledName, out mNullAble, out filed);
                    w.FiledName = filed;
                    q = Regex.Replace(w.VarName, @"(?:request|session|cookie|call)\.", string.Empty, RegexOptions.IgnoreCase);
                    vname = string.Format("{0}_{1}", dataType, q);
                    query = this.Config.tagregex.RequestValue.IsMatch(w.VarName) ? "\"\" + Request.QueryString"
                        : this.Config.tagregex.SessionValue.IsMatch(w.VarName) ? "\"\" + Session"
                        : this.Config.tagregex.CookieValue.IsMatch(w.VarName) ? "\"\" + Request.Cookies"
                        : this.Config.tagregex.CallValue.IsMatch(w.VarName) ? "\"\" + this.CallString" :
                        "";
                    if (requests.Contains(vname))
                    {
                        w.VarName = vname;
                        continue;
                    }
                    if (dataType == "string")
                    {
                        sb.AppendFormat("{0}string {1} = {2}[\"{3}\"];\r\n", Method.getSpaces(2), vname, query, q);
                    }
                    else if (dataType == "bool")
                    {
                        sb.AppendFormat("{0}bool {1} = {2}[\"{3}\"] == \"true\";\r\n", Method.getSpaces(2), vname, query, q);
                    }
                    else
                    {
                        sb.AppendFormat("{0}{1} {2} = {1}.MinValue;\r\n", Method.getSpaces(2), dataType, vname);
                        sb.AppendFormat("{0}{1}.TryParse({2}[\"{3}\"],out {4});\r\n", Method.getSpaces(2), dataType, query, q, vname);
                    }
                    w.VarName = vname;
                    requests.Add(vname);
                }
                else if (!w.VarName.Contains("DateTime.Now") && Regex.Match(w.VarName, @"^[a-zA-Z_]\w+(?:\.\w+)+", RegexOptions.IgnoreCase).Success)
                {
                    continue;
                }
                else
                {
                    dataType = GetObjType(model, w.FiledName, out mNullAble, out filed);
                    w.FiledName = filed;

                    vname = string.Format("{0}_{1}_{2}", dataType, filed, i);
                    if (dataType == "string")
                    {
                        if (!Regex.IsMatch(w.VarName, @""".*?"""))
                        {
                            w.VarName = string.Concat("\"", w.VarName, "\"");
                        }
                    }
                    else if ("int|long".Contains(dataType))
                    {
                        m = Regex.Match(w.VarName, @"^\-?\d+$");
                        if (!m.Success)
                        {
                            w.VarName = string.Format("-1/*--( {0} )不是有效的整数值，已设为默认值 -1--*/", w.VarName);
                        }
                    }
                    else if ("double|float|decimal".Contains(dataType))
                    {
                        m = Regex.Match(w.VarName, @"^\-?\d+(\.\d+)?$");
                        if (!m.Success)
                        {
                            w.VarName = string.Format("-1/*--( {0} )不是有效的小数值，已设为默认值 -1--*/", w.VarName);
                        }
                    }
                    else if (dataType == "DateTime")
                    {
                        vname = string.Format("{0}_{1}_{2}", dataType, filed, i);
                        GroupCollection gc = null;
                        m = Regex.Match(w.VarName,
            @"^(?<year>\d{4})[/\-](?<month>[0|1]?[0-2])[/\-](?<day>[0-2]?\d|3[0|1])[/\-]?(?<hour>[0|1]?\d|2[0-3]):?(?<min>[0-5]?\d):?(?<sec>[0-5]?\d)$");
                        if (m.Success)
                        {
                            gc = m.Groups;
                            sb.AppendFormat("{0}DateTime {1} = DateTime.Parse(\"{2}-{3}-{4} {5}:{6}:{7}\");\r\n",
                                    Method.getSpaces(2), vname, gc["year"].Value, gc["month"].Value, gc["day"].Value, gc["hour"].Value, gc["min"].Value, gc["sec"].Value);
                            w.VarName = vname;
                            continue;
                        }
                        m = Regex.Match(w.VarName, @"^(?<year>\d{4})[/\-](?<month>[0|1][0-2])[/\-](?<day>[0-2]?\d|3[0|1])$");
                        if (m.Success)
                        {
                            gc = m.Groups;
                            sb.AppendFormat("{0}DateTime {1} = DateTime.Parse(\"{2}-{3}-{4} 00:00:00\");\r\n",
                                    Method.getSpaces(2), vname, gc["year"].Value, gc["month"].Value, gc["day"].Value);
                            w.VarName = vname;
                            continue;
                        }
                        m = Regex.Match(w.VarName, @"^DateTime\.Now$");
                        if (m.Success)
                        {
                            sb.AppendFormat("{0}DateTime {1} = DateTime.Now;\r\n",
                                   Method.getSpaces(2), vname);
                            w.VarName = vname;
                            continue;
                        }
                        m = Regex.Match(w.VarName, @"^DateTime\.Now\.Add\w+\(-?\d+\)$");
                        if (m.Success)
                        {
                            sb.AppendFormat("{0}DateTime {1} = {2};\r\n",
                                   Method.getSpaces(2), vname, m.Value);
                            w.VarName = vname;
                            continue;
                        }
                        w.VarName =
                            string.Concat(
                            "DateTime.MinValue/*--( ", w.VarName, " )不是有效的日期或日期运算，请检查格式或数值。",
                            "（[年4位][-或/][月1或2位][-或/][日1或2位][/或-或空格 可有可无][时分秒均为1或2位可有可无]）。已设为默认值 DateTime.MinValue--*/"
                             );
                    }
                    else if (dataType == "bool")
                    {
                        w.VarName = w.VarName == "true" ? "true" : w.VarName == "false" ? "false"
                            : string.Format("false/*--( {0} )不是有效的bool值，已设为默认值 false--*/", w.VarName);
                    }
                }
            }
            return sb;
        }

        #endregion

        #region GetWheres
        private void GetWheres(List<TagWhere> wheres, string _params, string _symb)
        {
            string[] arr = _params.Split('&');
            string[] p = null;//拆分键值对
            bool or = false;
            string orStr = "";
            string field_Var = "";
            for (int i = 0; i < arr.Length; i += 1)
            {
                field_Var = arr[i];
                or = field_Var.IndexOf("|") != -1;
                if (or)
                {
                    p = field_Var.Split('|');
                    field_Var = p[0];
                    orStr = p.Length == 2 ? p[1] : string.Join("|", p.Skip(1));
                }

                if (field_Var.IndexOf("!=") != -1)
                {
                    p = field_Var.Replace("!=", "|").Split('|');
                    wheres.Add(new TagWhere(p[0], "!=", _symb, p[1]));
                }
                else if (field_Var.IndexOf("<=") != -1)
                {
                    p = field_Var.Replace("<=", "|").Split('|');
                    wheres.Add(new TagWhere(p[0], "<=", _symb, p[1]));
                }
                else if (field_Var.IndexOf(">=") != -1)
                {
                    p = field_Var.Replace(">=", "|").Split('|');
                    wheres.Add(new TagWhere(p[0], ">=", _symb, p[1]));
                }
                else if (field_Var.IndexOf("=") != -1)
                {
                    p = field_Var.Split('=');
                    wheres.Add(new TagWhere(p[0], "==", _symb, p[1]));
                }
                else if (field_Var.IndexOf("<") != -1)
                {
                    p = field_Var.Split('<');
                    wheres.Add(new TagWhere(p[0], "<", _symb, p[1]));
                }
                else if (field_Var.IndexOf(">") != -1)
                {
                    p = field_Var.Split('>');
                    wheres.Add(new TagWhere(p[0], ">", _symb, p[1]));
                }
                else if (field_Var.IndexOf("!%") != -1)
                {
                    p = field_Var.Replace("!%", "|").Split('|');
                    wheres.Add(new TagWhere(p[0], "!%", _symb, p[1]));
                }
                else if (field_Var.IndexOf("%") != -1)
                {
                    p = field_Var.Split('%');
                    wheres.Add(new TagWhere(p[0], "%", _symb, p[1]));
                }
                if (or)
                {
                    GetWheres(wheres, orStr, " || ");
                }
            }
        }

        #endregion

        #region Linq_getList

        public string Linq_getList(string tableName, string baseParms, HashSet<string> fields,
            out string modType, string upDataName, out string UpdataType, PagerTag pager)
        {
            UpdataType = "";
            modType = "";
            object model = GetModObj(tableName);
            if (model != null)
            {
                modType = model.GetType().Name;
            }
            else
            {
                return string.Concat(Method.getSpaces(2), "//不存在该表_", tableName, "\r\n");
            }
            if (!string.IsNullOrEmpty(upDataName))
            {
                UpperDataModel = GetModObj(upDataName);
                if (UpperDataModel != null)
                {
                    UpdataType = UpperDataModel.GetType().Name;
                }
            }

            StringBuilder linq = new StringBuilder(Method.getSpaces(2) + "/*" + baseParms + "*/\r\n");
            List<TagWhere> baseWheres = new List<TagWhere>();
            GetWheres(baseWheres, baseParms, " && ");
            List<string> orderbylist = new List<string>();
            bool desc = true;
            int pagesize = 0;//每页显示条数
            int take = 0;
            int skip = 0;
            #region where 筛选
            if (baseWheres.Count > 0)
            {
                linq.Append(GetWhereParams(model, baseWheres, baseParms));
                linq.AppendFormat("{0}var list = from a in {1}", Method.getSpaces(2), Config.GetingTableStr("list", modType));//拼接查询的linq语句
                bool first = true;
                foreach (var w in baseWheres)
                {
                    //剔除特殊条件 orderby 和 desc
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
                    else if (w.FiledName == "pagesize")
                    {
                        int.TryParse(w.VarName, out pagesize);
                        continue;
                    }
                    else if (w.FiledName == "take")
                    {
                        int.TryParse(w.VarName, out take);
                        continue;
                    }
                    else if (w.FiledName == "skip")
                    {
                        int.TryParse(w.VarName, out skip);
                        continue;
                    }
                    else if (w.FiledName == "item" || w.FiledName == "emptytext")
                    {
                        continue;
                    }
                    if (first)
                    {
                        linq.AppendFormat("\r\n{0}.Where( b=>\r\n {0}", Method.getSpaces(4));
                        w.LogicSymb = Method.getSpaces(1);
                        first = false;
                    }
                    if (w.FiledName == "null" || w.FiledName == "empty" || w.FiledName == "none")
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
                    linq.Append(")");
                }
                if (orderbylist.Count() == 0)
                {
                    orderbylist.Add(model.GetType().GetProperties().First().Name);
                }
                if (fields != null)
                {
                    foreach (string s in orderbylist)
                    {
                        fields.Add(s.ToLower());
                    }
                }
                if (fields != null && fields.Count > 0)
                {
                    linq.AppendFormat("\r\n{0}select new\r\n", Method.getSpaces(5));
                    linq.AppendFormat("{0}{1}", Method.getSpaces(5), "{\r\n");
                    int k = 0;
                    foreach (var s in fields)
                    {
                        k += 1;
                        linq.AppendFormat("{0}a.{1}{2}\r\n", Method.getSpaces(6), GetPropertyName(model, s), k == fields.Count ? "" : ",");
                    }
                    linq.Append(Method.getSpaces(5) + "};\r\n");
                }
                else
                {
                    linq.AppendFormat("\r\n{0}select a;\r\n", Method.getSpaces(5));
                }
            }
            #endregion
            if (pagesize <= 0 && take <= 0)
            {
                take = 99;
                linq.AppendFormat("{0}/*pagesize、take 都未指定，设置 take 为99;*/\r\n", Method.getSpaces(2));
            }
            linq.AppendFormat("{0}int totalsize = list.Count();\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}list = list", Method.getSpaces(2));//
            if (take > 0)
            {
                if (orderbylist.Count() > 0)
                {
                    linq.AppendFormat(".{0}(c=>c.{1})", desc ? "OrderByDescending" : "OrderBy", GetPropertyName(model, orderbylist[0]));
                    if (orderbylist.Count() > 1)
                    {
                        for (int i = 1; i < orderbylist.Count(); i += 1)
                        {
                            linq.AppendFormat("\r\n{0}.{1}(c=>c.{2})", Method.getSpaces(5), desc ? "ThenByDescending" : "ThenBy", GetPropertyName(model, orderbylist[i]));
                        }
                    }
                }
                if (skip > 0)
                {
                    linq.AppendFormat(".Skip({1})", 0, skip);//
                }
                linq.AppendFormat(".Take({1});\r\n", 0, take);//
                linq.AppendFormat("{0}/*不分页，显示前 {1}条数据;*/\r\n", Method.getSpaces(2), take);
            }
            else
            {
                #region orderby 排序分页
                if (orderbylist.Count() > 0)
                {
                    linq.AppendFormat(".{0}(c=>c.{1})", desc ? "OrderByDescending" : "OrderBy", GetPropertyName(model, orderbylist[0]));
                    if (orderbylist.Count() > 1)
                    {
                        for (int i = 1; i < orderbylist.Count(); i += 1)
                        {
                            linq.AppendFormat("\r\n{0}.{1}(c=>c.{2})", Method.getSpaces(5), desc ? "ThenByDescending" : "ThenBy", GetPropertyName(model, orderbylist[i]));
                        }
                    }
                }
                linq.AppendFormat(".Skip((page - 1) * {0}).Take({0});\r\n", pagesize);//
                if (skip > 0)
                {
                    linq.AppendFormat("{0}/*无效的skip参数，分页时该参数无效！\r\n", Method.getSpaces(2));//
                }
                linq.AppendFormat("{0}/*分页，每页显示{1};*/\r\n", Method.getSpaces(2), pagesize);
                if (pager != null)
                {
                    if (pager.type == PagerType.cs)
                    {
                        linq.AppendFormat("{0}Pager pager= new Pager(totalsize, page, {1},Request.RawUrl, {2}, \"{3}\", \"{4}\", \"{5}\", {6});\r\n",
                            Method.getSpaces(2), pagesize, pager.Num_edge, pager.Prev_text, pager.Next_text, pager.Ellipse_text, pager.PrevOrNext_show ? "true" : "fase");
                        linq.AppendFormat("{0}{1}.Text = \"<div id='pager'>\" + pager.MakeLinks() +\"</div>\"; \r\n", Method.getSpaces(2), pager.GetTagName());
                    }
                    else
                    {
                        linq.AppendFormat("{0}string urlparams = RemovePageParams(Request.RawUrl);\r\n", Method.getSpaces(2));
                        linq.AppendFormat(
                            "{0}{2}.Text = \"<div id='pagerinfo' style='display:none;'>\"\r\n{1}+ \"{3}\"\r\n{1}+ \"{4}\"\r\n{1}+ \"{5}\"\r\n{1} + \"{6}\"\r\n{1}+ \"\\r\\n</div>\";\r\n",
                            Method.getSpaces(2), Method.getSpaces(4), pager.GetTagName(),
                           "\\r\\n<input type='hidden' id='list_size' value='\" + totalsize + \"' data-info='总纪录条数' />",
                            "\\r\\n<input type='hidden' id='num_per_page' value='\" + " + pagesize + " + \"' data-info='每页显示' />",
                            "\\r\\n<input type='hidden' id='current_page' value='\" + page + \"' data-info='当前页' />",
                            "\\r\\n<input type='hidden' id='current_url_params' value='\" + urlparams + \"' data-info='当前url参数（不带page参数）' />"
                            );
                    }
                }
                else
                {
                    linq.AppendFormat("{0}/*无法分页，未设置分页标签！;*/\r\n", Method.getSpaces(2));
                }
                #endregion
            }

            return linq.ToString();
        }

        #endregion

        #region Linq_getJson
        /// <summary>
        /// 拼接Json查询语句
        /// </summary>
        /// <param name="tableName">要获取数据的表名称</param>
        /// <param name="baseParms">标签参数</param>
        /// <param name="modType">输出表对应实体的类型</param>
        /// <param name="tagName">查询条件</param>
        /// <returns></returns>
        public string Linq_getJson(string tableName, string baseParms, out string modType, string tagName)
        {
            modType = "";
            object model = GetModObj(tableName);
            if (model != null)
            {
                modType = model.GetType().Name;
            }
            else
            {
                return string.Concat(Method.getSpaces(2), "//不存在该表_", tableName, "\r\n");
            }
            StringBuilder linq = new StringBuilder(Method.getSpaces(2) + "/*" + baseParms + "*/\r\n");
            linq.AppendFormat("{0}string jsonname = this.CallValue(\"jsonname\");\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}if (jsonname != \"{1}\")\r\n", Method.getSpaces(2), tagName);
            linq.Append(Method.getSpaces(2) + "{\r\n");
            linq.Append(Method.getSpaces(3) + "return null;\r\n");
            linq.Append(Method.getSpaces(2) + "}\r\n");
            List<TagWhere> baseWheres = new List<TagWhere>();
            GetWheres(baseWheres, baseParms, " && ");
            List<string> orderbylist = new List<string>();
            bool desc = true;
            int pagesize = 0;//每页显示条数
            int take = 0;
            HashSet<string> fields = new HashSet<string>();
            #region where 筛选
            if (baseWheres.Count > 0)
            {
                linq.Append(GetWhereParams(model, baseWheres, baseParms));
                linq.AppendFormat("{0}var list = from a in {1}", Method.getSpaces(2), Config.GetingTableStr("json", modType));//拼接查询的linq语句
                bool first = true;
                foreach (var w in baseWheres)
                {
                    //剔除特殊条件 orderby 和 desc
                    if (w.FiledName == "orderby")
                    {
                        var list = w.VarName.Split(',').Where(x => x.Length > 0);
                        foreach (var x in list)
                        {
                            orderbylist.Add(GetPropertyName(model, x));
                        }
                        continue;
                    }
                    else if (w.FiledName == "desc")
                    {
                        desc = w.VarName.ToLower() == "true";
                        continue;
                    }
                    else if (w.FiledName == "pagesize")
                    {
                        int.TryParse(w.VarName, out pagesize);
                        continue;
                    }
                    else if (w.FiledName == "take")
                    {
                        int.TryParse(w.VarName, out take);
                        continue;
                    }
                    else if (w.FiledName == "page")
                    {
                        continue;
                    }
                    else if (w.FiledName == "item")
                    {
                        continue;
                    }
                    else if (w.FiledName == "fields")
                    {
                        var names = w.VarName.ToLower().Split(',');
                        foreach (var x in names)
                        {
                            fields.Add(x.ToLower());
                        }
                        continue;
                    }
                    if (first)
                    {
                        linq.AppendFormat("\r\n{0}.Where( b=>\r\n {0}", Method.getSpaces(4));
                        w.LogicSymb = Method.getSpaces(1);
                        first = false;
                    }
                    if (w.FiledName == "null" || w.FiledName == "empty" || w.FiledName == "none")
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
                    linq.Append(")");
                }
                if (orderbylist.Count() == 0)
                {
                    orderbylist.Add(model.GetType().GetProperties().First().Name);
                }
                if (fields != null)
                {
                    foreach (string s in orderbylist)
                    {
                        if (!fields.Contains(s.ToLower()))
                        {
                            fields.Add(s);
                        }
                    }
                }
                if (fields.Count < 1)
                {
                    fields = GetFields(tableName);
                }
                HashSet<string> _fields = new HashSet<string>();
                foreach (var x in fields)
                {
                    _fields.Add(GetPropertyName(model, x));
                }
                fields.Clear();
                foreach (var x in _fields)
                {
                    fields.Add(x);
                }
                linq.AppendFormat("\r\n{0}select new\r\n", Method.getSpaces(5));
                linq.AppendFormat("{0}{1}", Method.getSpaces(5), "{\r\n");
                int k = 0;
                foreach (var s in fields)
                {
                    k += 1;
                    linq.AppendFormat("{0}a.{1}{2}\r\n", Method.getSpaces(6), GetPropertyName(model, s), k == fields.Count ? "" : ",");
                }
                linq.Append(Method.getSpaces(5) + "};\r\n");
            }
            #endregion
            #region orderby 排序分页
            if (pagesize <= 0)
            {
                pagesize = 15;
                linq.AppendFormat("{0}/*pagesize未指定，默认分页大小为15;*/\r\n", Method.getSpaces(2));
            }
            if (take > 0)
            {
                linq.AppendFormat("{0}/*无效的参数take，该参数对jsonTag无效;*/\r\n", Method.getSpaces(2));
            }

            linq.AppendFormat("{0}int __page = 0;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}int.TryParse(this.CallValue(\"page\"), out __page);\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}__page = __page < 1 ? 1 : __page;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}int totalsize = list.Count();\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}list = list", Method.getSpaces(2));//
            if (orderbylist.Count() > 0)
            {
                linq.AppendFormat(".{0}(c=>c.{1})", desc ? "OrderByDescending" : "OrderBy", GetPropertyName(model, orderbylist[0]));
                if (orderbylist.Count() > 1)
                {
                    for (int i = 1; i < orderbylist.Count(); i += 1)
                    {
                        linq.AppendFormat("\r\n{0}.{1}(c=>c.{2})", Method.getSpaces(5), desc ? "ThenByDescending" : "ThenBy", GetPropertyName(model, orderbylist[i]));
                    }
                }
            }
            linq.AppendFormat(".Skip((__page - 1) * {0}).Take({0});\r\n", pagesize);
            linq.AppendFormat("{0}dynamic json = new ExpandoObject();\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}json.jsonname = \"{1}\";\r\n", Method.getSpaces(2), tagName);
            linq.AppendFormat("{0}json.tagstr = \"{1}\";\r\n", Method.getSpaces(2), baseParms);
            linq.AppendFormat("{0}json.skip = (__page - 1) * {1};\r\n", Method.getSpaces(2), pagesize);
            linq.AppendFormat("{0}json.pagesize = {1};\r\n", Method.getSpaces(2), pagesize);
            linq.AppendFormat("{0}json.listsize = totalsize;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}json.page = __page;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}json.over = list.Count() < {1};\r\n", Method.getSpaces(2), pagesize);
            linq.AppendFormat("{0}json.data = list;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}CallbackResult call = new CallbackResult(json) ;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}call.type = \"jsoncall\";\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}return call;\r\n", Method.getSpaces(2), pagesize);
            #endregion
            return linq.ToString();
        }
        #endregion

        #region Linq_getRead
        public string Linq_getRead(string tableName, string query, out string modType, string upDataName, string readname)
        {
            object model = GetModObj(tableName);
            if (model != null)
            {
                modType = model.GetType().Name;
            }
            else
            {
                modType = "";
                return Method.getSpaces(2) + "//不存在该表_" + tableName + "\r\n";
            }
            if (!string.IsNullOrEmpty(upDataName))
            {
                UpperDataModel = GetModObj(upDataName);
            }
            StringBuilder linq = new StringBuilder(Method.getSpaces(2) + "/*" + query + "*/\r\n");
            List<TagWhere> baseWheres = new List<TagWhere>();
            GetWheres(baseWheres, query, " && ");
            if (baseWheres.Count > 0)
            {
                linq.Append(GetWhereParams(model, baseWheres, query));
                bool first = true;
                List<string> orderbylist = new List<string>();
                bool desc = true;
                foreach (var w in baseWheres)
                {
                    //剔除特殊条件 orderby 和 desc
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
                    if (first)
                    {
                        linq.AppendFormat("{0}{1} = {2}\r\n{3}.Where( b=>\r\n {3}", Method.getSpaces(2), readname,
                            Config.GetingTableStr("read", modType), Method.getSpaces(4));
                        w.LogicSymb = Method.getSpaces(1);
                        first = false;
                    }
                    if (w.FiledName == "null" || w.FiledName == "empty" || w.FiledName == "none")
                    {
                        linq.AppendFormat("{0}{1}{2} {3}\r\n{4}", w.LogicSymb, w.FieldLeft, w.VarName, w.VarRight, Method.getSpaces(5));
                    }
                    else if (w.Compare == "%")
                    {
                        linq.AppendFormat("{0}{1}b.{2}.Contains({3}){4}\r\n{5}", w.LogicSymb, w.FieldLeft,
                            w.FiledName, w.VarName, w.VarRight, Method.getSpaces(5));

                    }
                    else if (w.Compare == "!%")
                    {
                        linq.AppendFormat("{0}{1}!b.{2}.Contains({3}){4}\r\n{5}", w.LogicSymb, w.FieldLeft,
                            w.FiledName, w.VarName, w.VarRight, Method.getSpaces(5));
                    }
                    else
                    {
                        linq.AppendFormat("{0}{1}b.{2} {3} {4}{5}\r\n{6}", w.LogicSymb, w.FieldLeft, w.FiledName,
                            w.Compare, w.VarName, w.VarRight, Method.getSpaces(5));
                    }
                }
                if (!first)
                {
                    if (orderbylist.Count() > 0)
                    {
                        linq.AppendFormat(").{0}(c=>c.{1})", desc ? "OrderByDescending" : "OrderBy", GetPropertyName(model, orderbylist[0]));
                        if (orderbylist.Count() > 1)
                        {
                            for (int i = 1; i < orderbylist.Count(); i += 1)
                            {
                                linq.AppendFormat("\r\n{0}.{1}(c=>c.{2})", Method.getSpaces(5), desc ? "ThenByDescending" : "ThenBy", GetPropertyName(model, orderbylist[i]));
                            }
                        }
                        linq.AppendFormat("\r\n{0}.FirstOrDefault();\r\n", Method.getSpaces(4));//
                    }
                    else
                    {
                        linq.AppendFormat(")\r\n{0}.FirstOrDefault();\r\n", Method.getSpaces(4));//
                    }
                }

                linq.AppendFormat("{0}if ({1} == null)\r\n", Method.getSpaces(2), readname);
                linq.Append(Method.getSpaces(2) + "{\r\n");
                linq.AppendFormat(Method.getSpaces(3) + "{0} = new {1}();\r\n", readname, modType);
                linq.Append(Method.getSpaces(2) + "}\r\n");
            }
            return linq.ToString();
        }

        #endregion

        #region Linq_getForm

        internal string Linq_Form(string tableName, object model, string BaseParams, List<FromVar> Vars, string CallName)
        {
            string modType = model.GetType().Name;
            string dataName = "_" + modType.ToLower();
            string allowempty = "", action = "both";
            List<TagWhere> baseWheres = new List<TagWhere>();
            GetWheres(baseWheres, BaseParams, " && ");
            StringBuilder linq = new StringBuilder(Method.getSpaces(2) + "/*" + BaseParams + "*/\r\n");
            linq.AppendFormat("{0}string formname = this.CallValue(\"formname\");\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}if (formname != \"{1}\")\r\n", Method.getSpaces(2), CallName);
            linq.Append(Method.getSpaces(2) + "{\r\n");
            linq.Append(Method.getSpaces(3) + "return null;\r\n");
            linq.Append(Method.getSpaces(2) + "}\r\n");
            string vname = "";
            //
            linq.AppendFormat("{0}dynamic error = new ExpandoObject();\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}error.formname = formname;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}CallbackResult call = new CallbackResult(error) ;\r\n", Method.getSpaces(2));
            linq.AppendFormat("{0}call.type = \"formcall\";\r\n", Method.getSpaces(2));

            TagWhere add = baseWheres.FirstOrDefault(x => x.FiledName == "action");
            if (add != null)
            {
                action = add.VarName;
            }
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
                        linq.Append(Helper.GetWhereParams(model, baseWheres, BaseParams));
                        linq.AppendFormat("\r\n{0}{1} = {2}.FirstOrDefault( b=>\r\n {3}", Method.getSpaces(2),
                            dataName, Config.GetingTableStr("form", modType), Method.getSpaces(4));
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
                        linq.AppendFormat(".{0}(c=>c.{1})", desc ? "OrderByDescending" : "OrderBy", GetPropertyName(model, orderbylist[0]));
                        if (orderbylist.Count() > 1)
                        {
                            for (int i = 1; i < orderbylist.Count(); i += 1)
                            {
                                linq.AppendFormat("\r\n{0}.{1}(c=>c.{2})", Method.getSpaces(5), desc ? "ThenByDescending" : "ThenBy",
                                    GetPropertyName(model, orderbylist[i]));
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
                    linq.AppendFormat("{0}Db_Context.{1}.{2}({3});\r\n", Method.getSpaces(3), modType, Helper.GetAddMethod(modType), dataName);
                }
                linq.Append(Method.getSpaces(2) + "}\r\n");

            }
            else
            {
                linq.AppendFormat("{0}/*添加模式（action = both），新建记录*/;\r\n", Method.getSpaces(3));
                linq.AppendFormat("{0}{1} {2} = new {1}();\r\n", Method.getSpaces(2), modType, dataName);
                linq.AppendFormat("{0}Db_Context.{1}.{2}({3});\r\n", Method.getSpaces(2), modType, Helper.GetAddMethod(modType), dataName);
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
                        linq.AppendFormat("{0}error.dom = \"{1}\";\r\n", Method.getSpaces(3), v.Name);
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
                        linq.AppendFormat("{0}error.dom = \"{1}\";\r\n", Method.getSpaces(4), v.Name);
                        linq.AppendFormat("{0}return call;\r\n", Method.getSpaces(4));
                        linq.Append(Method.getSpaces(3) + "}\r\n");
                        linq.Append(Method.getSpaces(3) + "else\r\n");
                        linq.Append(Method.getSpaces(3) + "{\r\n");
                        linq.AppendFormat("{0}error.code = 2;\r\n", Method.getSpaces(4));
                        linq.AppendFormat("{0}error.msg = \"请输入正确的{1}\";\r\n", Method.getSpaces(4),
                            v.Type == "DateTime" ? "时间" : "int|long".Contains(v.Type) ? "整数" : "小数");
                        linq.AppendFormat("{0}error.dom = \"{1}\";\r\n", Method.getSpaces(4), v.Name);
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

        #endregion

        #region some methods

        /// <summary>
        /// 根据表名返回一个表的实例 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实例</returns>
        public object GetModObj(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }
            Type ts = this.Config.db.GetType();
            //查找表
            PropertyInfo pi = ts.GetProperties().FirstOrDefault(a => a.Name.ToLower() == tableName.ToLower());
            if (pi == null)//找不到表，查找方言
            {
                foreach (var d in Config.TableDialects)
                {
                    if (d.MatchName(tableName))
                    {
                        tableName = d.Name;
                        pi = ts.GetProperties().FirstOrDefault(a => a.Name.ToLower() == tableName.ToLower());
                        break;
                    }
                }
            }
            if (pi == null)
            {
                return Config.GetModObj(tableName);
            }
            var v = pi.GetValue(this.Config.db, null);  //获取一个 ObjectSet<> 或 DbSet<>
            //ObjectSet<>             DbSet<>
            object model = v.GetType().GetMethods().FirstOrDefault(a => a.Name == "CreateObject" || a.Name == "Create").Invoke(v, null);
            return model;
        }

        internal string GetAddMethod(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return "";
            }
            Type ts = this.Config.db.GetType();
            PropertyInfo pi = ts.GetProperties().FirstOrDefault(a => a.Name.ToLower() == tableName.ToLower());
            if (pi == null)
            {
                return null;
            }
            var v = pi.GetValue(this.Config.db, null);  //获取一个 ObjectSet<> 或 DbSet<>
            //ObjectSet<>             DbSet<>
            return v.GetType().GetMethods().FirstOrDefault(a => a.Name == "AddObject" || a.Name == "Add").Name;
        }

        internal string GetModFieldName(string tableName, string fieldName)
        {
            object model = GetModObj(tableName);
            if (model == null)
            {
                return string.Format("/*{0} 不存在的表 */{1}", model, tableName);
            }
            return GetPropertyName(model, fieldName);
        }

        /// <summary>
        /// 根据表名称推测正确的表名
        /// </summary>
        /// <param name="tableName">原表名（可能大小写不对）</param>
        /// <returns>正确的表名</returns>
        internal string GetTableName(string tableName)
        {
            var tb = GetModObj(tableName);
            if (tb != null)
            {
                return tb.GetType().Name;
            }
            return string.Format("/*{0} 不存在的表{1} */", "" + tableName);
        }

        internal HashSet<string> GetFields(string tableName)
        {
            object model = GetModObj(tableName);
            HashSet<string> names = new HashSet<string>();
            if (model != null)
            {
                PropertyInfo[] ps = model.GetType().GetProperties();
                foreach (PropertyInfo p in ps)
                {
                    if (p.Name == "EntityState" || p.Name == "EntityKey")
                    {
                        continue;
                    }
                    names.Add(p.Name.ToLower());
                }
            }
            return names;
        }

        /// <summary>
        /// 根据给定的属性名称返回属性类型
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="name">字段名称</param>
        /// <param name="isNullAble">是否为NullAble</param>
        /// <param name="newName">新的名称（纠正大小写）</param>
        /// <returns>字段类型</returns>
        internal string GetObjType(object obj, string name, out bool isNullAble, out string newName)
        {
            isNullAble = false;
            newName = name;
            if (obj == null)
            {
                return "/*obj 不能为空*/";
            }

            PropertyInfo pi = GetProperty(obj, name);
            if (pi == null)
            {
                return string.Concat("/* ", obj, "不存在该字段 [", name, "]*/\r\nstring");
            }
            Type columnType = pi.PropertyType;
            newName = pi.Name;
            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                columnType = columnType.GetGenericArguments()[0];
                isNullAble = true;
            }
            if (dataTypes.Keys.Contains(columnType.Name))
            {
                return dataTypes[columnType.Name];
            }
            return columnType.Name;
        }

        /// <summary>
        /// 获取字段的名称
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="name">字段名称（不区分大小写）</param>
        /// <returns>字段的真实名称（区分大小写）</returns>
        internal string GetPropertyName(object obj, string name)
        {
            if (obj == null)
            {
                return "/*obj 不能为空*/";
            }
            PropertyInfo pi = GetProperty(obj, name);
            if (pi == null)
            {
                return string.Concat("/* ", obj.GetType().Name, "不存在该字段 [", name, "]*/\r\nstring");
            }
            return pi.Name;
        }


        /// <summary>
        /// 根据字段名称获取字段
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="name">字段名称</param>
        /// <returns>字段信息</returns>
        private PropertyInfo GetProperty(object obj, string name)
        {
            if (obj == null || name == null)
            {
                return null;
            }
            var pi = obj.GetType().GetProperties().FirstOrDefault(a => a.Name.ToLower() == name.ToLower());
            if (pi == null)
            {
                var tableName = obj.GetType().Name;
                foreach (var d in Config.TableDialects)
                {
                    if (d.MatchName(tableName))
                    {
                        tableName = d.Name;
                        foreach (var f in d.Fields)
                        {
                            if (f.MatchName(name))
                            {
                                pi = obj.GetType().GetProperties().FirstOrDefault(a => a.Name.ToLower() == f.Name.ToLower());
                                return pi;
                            }
                        }
                        break;
                    }
                }
            }
            return pi;
        }
        #endregion
    }
}