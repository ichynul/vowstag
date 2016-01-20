
using Tag.Vows.Bean;
namespace Tag.Vows.Interface
{
    interface ICallBackControl
    {
        CallBackResult DoCallBack();
        CallBackResult TagCallBack();
        string CallValue(string key);
    }
}
