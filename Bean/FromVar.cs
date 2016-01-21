
namespace Tag.Vows.Web
{
    class FromVar
    {
        public string Name { get; set; }
        public string OName { get; set; }
        public string Type { get; set; }
        public bool NullAble { get; set; }

        private FromVar() { }

        public FromVar(string mName, string mOName, string mType, bool mNullAble)
        {
            this.Name = mName;
            this.OName = mOName;
            this.Type = mType;
            this.NullAble = mNullAble;
        }
    }
}
