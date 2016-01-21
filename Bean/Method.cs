using System.Text;

namespace Tag.Vows.Web
{
    /// <summary>
    ///Method 的摘要说明
    /// </summary>
    class Method
    {
        public Method()
        {
            body = new StringBuilder();
        }
        public string name { get; set; }
        public string returnType { get; set; }
        public StringBuilder body { get; set; }
        public bool in_page_load { get; set; }

        public string parsmstr { get; set; }

        public string use_parsm { get; set; }

        public static string space = "    ";

        public string ToFullMethodRect()
        {
            return string.Concat(space, "public ", returnType == null ? "void" : returnType, " ", name, "(", parsmstr, ")\r\n"
                , space, "{\r\n", body, space,
                "}\r\n\r\n");
        }
        public static string getSpaces(int times)
        {
            if (times < 2)
            {
                return times == 1 ? space : "";
            }
            string str = space;
            for (int i = 1; i < times; i += 1)
            {
                str += space;
            }

            return str;
        }

    }
}