
using System.Collections.Generic;
using Tag.Vows.Enum;
namespace Tag.Vows.Interface
{
    interface IMethodDataAble
    {
        void SetDataName(string dataName, MethodType type);
        HashSet<string> GetFieldName();
    }
}
