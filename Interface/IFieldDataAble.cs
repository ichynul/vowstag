
using Tag.Vows.Enum;
namespace Tag.Vows.Interface
{
    interface IFieldDataAble
    {
        void SetDataName(string dataName, FieldType type);
        string GetFieldName();
    }
}
