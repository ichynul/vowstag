
namespace Tag.Vows
{
    public class CallBackResult
    {
        private CallBackResult() { }

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
