namespace Tag.Vows
{
    interface IHtmlAble : IMakeAble
    {
        string getAspxCode();
        string ToPageString();
        string GetPageName();
    }
}
