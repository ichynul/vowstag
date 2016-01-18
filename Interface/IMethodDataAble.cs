
using System.Collections.Generic;
namespace Tag.Vows
{
    interface IMethodDataAble
    {
        void setDataName(string DataName, MethodType type);
        HashSet<string> getFieldName();
    }
}
