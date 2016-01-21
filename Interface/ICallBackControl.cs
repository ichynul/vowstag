
using Tag.Vows.Web;
namespace Tag.Vows.Interface
{
    interface ICallBackControl
    {
        CallBackResult DoCallBack();
        CallBackResult TagCallBack();
        string CallValue(string key);
    }
}
