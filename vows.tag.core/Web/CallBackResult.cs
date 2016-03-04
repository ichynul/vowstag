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
namespace Tag.Vows.Web
{
    /// <summary>
    /// tagCall处理结果
    /// </summary>
    public class CallBackResult
    {
        /// <summary>
        /// CallBackResult构造
        /// </summary>
        protected CallBackResult() { }

        /// <summary>
        ///  请求的结果
        /// </summary>
        /// <param name="result">请求的结果</param>
        public CallBackResult(object result)
        {
            this.result = result;
        }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string callstr = "";
        /// <summary>
        /// 请求类型
        /// </summary>
        public string type = "none";
        /// <summary>
        /// 请求类型
        /// </summary>
        public object result { get; private set; }
    }
}
