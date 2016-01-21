
namespace Tag.Vows.Web
{
    public class CallBackResult
    {
        protected CallBackResult() { }

        /// <summary>
        ///  请求的结果
        /// </summary>
        /// <param name="result">string 或 object</param>
        public CallBackResult(object result)
        {
            this.result = result;
        }

        public string type = "none";
        public object result { get; private set; }
    }
}
