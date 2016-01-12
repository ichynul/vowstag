using System.Web.UI;

namespace Tag.Vows
{
    interface ICallBackControl
    {
        CallBackResult DoCallBack();
        CallBackResult TagCallBack();
        string CallValue(string key);
    }
}
