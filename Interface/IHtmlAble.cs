namespace Tag.Vows.Interface
{
    interface IHtmlAble : IMakeAble
    {
        string GetAspxCode();
        string ToPageString();
        string GetPageName();
    }
}
