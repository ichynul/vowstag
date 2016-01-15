using System.Linq;
using System.Text.RegularExpressions;

namespace Tag.Vows
{
    class MethodTag : BaseTag, IMethodDataAble
    {
        private string Obj;
        private string Params;
        private string Method;
        private string Dataname = "";
        public MethodType Type { get; private set; }
        public string forname { get; set; }
        public MethodTag(string mtext, string mOrigin, int Deep, mPaths path, int no_)
            : base(mtext, mOrigin, Deep, path, no_)
        {
        }

        protected override void Discover()
        {
            this.Obj = this.path.tagregex.getMethodObj(this.Text);
            string[] arr = Obj.Split('(');
            this.Method = arr[0];
            this.Params = arr[1].Replace(")", string.Empty);
            if (Params.Contains("item."))
            {
                this.Type = MethodType.itemValue;
            }
            else if (this.Params.Contains("read."))
            {
                this.Type = MethodType.readValue;
            }
            else if (Method.Contains("form."))
            {
                this.Type = MethodType.formMethod;
            }
            else
            {
                this.Type = MethodType.normal;
            }
        }

        protected override string getCodeForAspx()
        {
            if (string.IsNullOrEmpty(this.Dataname) && this.Type == MethodType.itemValue)
            {
                string tag = Regex.Replace(Obj, @"(\{|\})", string.Empty, RegexOptions.IgnoreCase);
                string[] arr = tag.Split('(');
                string Method = arr[0];
                string itemField = "";
                string mParams = arr[1].Replace(")", string.Empty);
                if (mParams.Contains(','))
                {
                    arr = Params.Split(',');
                    string s = arr.FirstOrDefault(a => a.Contains("item."));
                    arr = s.Split('.');
                }
                else
                {
                    arr = Params.Split('.');
                }

                for (int j = 0; j < arr.Length; j += 1)
                {
                    if (arr[j] == "item" && j < arr.Length - 1)
                    {
                        itemField = arr[j + 1];
                        break;
                    }
                }
                return string.Format("<%# {0}({1}) %>", Method,
                    Params.Replace("item." + itemField, "Eval(\"" + itemField + "\")"));
            }
            else if (this.Type == MethodType.formMethod)
            {
                return string.Format("_tagcall.form('{0}'{1}); return false;", this.forname, this.Params.ToLower() == "false" ? " ,false" : " ,true");
            }
            else if (!string.IsNullOrEmpty(this.Dataname) && (this.Type == MethodType.itemValue || this.Type == MethodType.readValue))
            {
                string oldItemField = "";
                string itemField = "";
                string[] arr = null;
                if (this.Params.Contains(','))
                {
                    arr = this.Params.Split(',');
                    string s = arr.FirstOrDefault(a => a.Contains("read.") || a.Contains("item."));
                    arr = s.Split('.');
                }
                else
                {
                    arr = this.Params.Split('.');
                }
                for (int i = 0; i < arr.Length; i += 1)
                {
                    if ((arr[i] == "item" || arr[i] == "read") && i < arr.Length - 1)
                    {
                        oldItemField = arr[i + 1];
                    }
                }

                Dataname = TempleHelper.getTempleHelper(this.path).getTableName(Dataname);
                itemField = TempleHelper.getTempleHelper(this.path).getModFieldName(Dataname, oldItemField);
                if (!string.IsNullOrEmpty(oldItemField) && !string.IsNullOrEmpty(itemField))
                {
                    this.Obj = this.Obj.Replace(oldItemField, itemField);
                }
            }
            return string.Format("<% ={0} %>", this.Obj);
        }

        public override string toTagString()
        {
            return "【全局名称" + this.getTagName() + ",标签类型：Method，全名：" + this.Obj + "，方法：" + this.Method + "，参数：" + this.Params + "】<br />";
        }

        public void setDataName(string DataName, MethodType type)
        {
            if (this.Type == type)
            {
                this.Dataname = DataName;
            }
        }

        public string getFieldName()
        {
            if (this.Type == MethodType.itemValue)
            {
                string tag = Regex.Replace(Obj, @"(\{|\})", string.Empty, RegexOptions.IgnoreCase);
                string[] arr = tag.Split('(');
                string Method = arr[0];
                string itemField = "";
                string mParams = arr[1].Replace(")", string.Empty);
                if (mParams.Contains(','))
                {
                    arr = Params.Split(',');
                    string s = arr.FirstOrDefault(a => a.Contains("item."));
                    arr = s.Split('.');
                }
                else
                {
                    arr = Params.Split('.');
                }

                for (int j = 0; j < arr.Length; j += 1)
                {
                    if (arr[j] == "item" && j < arr.Length - 1)
                    {
                        itemField = arr[j + 1];
                        return itemField;
                    }
                }
            }

            return null;
        }
    }
}