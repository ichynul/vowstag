using System.Text.RegularExpressions;

namespace Tag.Vows
{
    class mWhere
    {
        private mWhere() { }
        public mWhere(string mFiledName, string mCompare, string mLogicSymb, string mVarName)
        {
            this.NotDataField = Regex.IsMatch(mFiledName, "^[#$%]");
            this.FiledName = Regex.Replace(mFiledName, @"\W", "").ToLower();
            this.Compare = mCompare;
            this.LogicSymb = mLogicSymb;
            this.VarName = Regex.Replace(mVarName, @"[^\w\-:\.,]", "");
            Match m = Regex.Match(mFiledName, @"(?<=^)[!\(]+(?=\w)");
            if (m.Success)
            {
                this.FieldLeft = m.Value;
            }
            m = Regex.Match(mVarName, @"(?<=\w)\)+(?=$)");
            if (m.Success)
            {
                this.VarRight = m.Value;
            }
        }

        public string FiledName { get; set; }
        public string Compare { get; set; }
        public string LogicSymb { get; set; }
        public string VarName { get; set; }
        public string FieldLeft { get; set; }
        public string VarRight { get; set; }
        public bool NotDataField { get; set; }
    }
}
