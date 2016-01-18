
using System.Collections.Generic;
namespace Tag.Vows
{
    interface IMethodDataAble
    {
        void SetDataName(string dataName, MethodType type);
        HashSet<string> GetFieldName();
    }
}
