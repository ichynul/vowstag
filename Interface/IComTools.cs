namespace Tag.Vows.Interface
{
    interface IComTools
    {
        string TimeFormat(object time);
        string TimeFormat(object time, string format);
        string FloatFormat(object number, string format);
        string SubString(object str, int length);
        string ValueOf(object str);
        string RemovePageParams(string url);
    }
}