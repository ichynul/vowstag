/*
* The MIT License (MIT)

*Copyright (c) 2015 ichynul

*Permission is hereby granted, free of charge, to any person obtaining a copy
*of this software and associated documentation files (the "Software"), to deal
*in the Software without restriction, including without limitation the rights
*to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*copies of the Software, and to permit persons to whom the Software is
*furnished to do so, subject to the following conditions:

*The above copyright notice and this permission notice shall be included in all
*copies or substantial portions of the Software.

*THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
*LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
*OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
*SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// DataHelper 的摘要说明
/// 2015/07/10
/// lianghaiyun
/// </summary>
/// 
namespace Tag.Vows.Data
{
    internal class DataHelper
    {
        private static Dictionary<string, string> _dataTypes;
        protected static Dictionary<string, string> dataTypes
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

        private DataHelper()
        {
        }
        #region 根据给定的属性名称返回属性类型
        /// <summary>
        /// 根据给定的属性名称返回属性类型
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="name">字段名称</param>
        /// <returns>字段类型</returns>
        internal static string GetType(object obj, string name, out bool isNullAble, out string newName)
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
                return string.Concat("/* ", obj, " 不存在该字段 [", name, "]*/\r\nstring");
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
        #endregion

        #region 获取字段的名称
        /// <summary>
        /// 获取字段的名称
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="name">字段名称（不区分大小写）</param>
        /// <returns>字段的真实名称（区分大小写）</returns>
        internal static string GetPropertyName(object obj, string name)
        {
            PropertyInfo pi = GetProperty(obj, name);
            if (pi == null)
            {
                return obj == null ? "/*obj 不能为空*/" : string.Format("/*{0} 不存在该字段!*/", obj.GetType().Name + "." + name);
            }
            return pi.Name;
        }
        #endregion
        #region 根据字段名称获取字段
        /// <summary>
        /// 根据字段名称获取字段
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="name">字段名称</param>
        /// <returns>字段信息</returns>
        internal static PropertyInfo GetProperty(object obj, string name)
        {
            if (obj == null || name == null)
            {
                return null;
            }
            return obj.GetType().GetProperties().FirstOrDefault(a => a.Name.ToLower() == name.ToLower());
        }

        #endregion
        #region 判断字段是否存在
        internal static bool HasProperty(object obj, string name)
        {
            return GetProperty(obj, name) != null;
        }
        #endregion

    }
}