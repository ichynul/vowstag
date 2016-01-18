namespace Tag.Vows
{
    interface IHtmlAble : IMakeAble
    {
        string GetAspxCode();
        string ToPageString();
        string GetPageName();
    }
}
