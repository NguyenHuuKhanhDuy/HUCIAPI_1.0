using System.ComponentModel;

namespace Common.Enums
{
    public enum TypeActionEnum
    {
        [Description("Thêm")]
        Add = 1,

        [Description("Xóa")]
        Delete = 2,

        [Description("Sửa")]
        Update = 3,
    }
}
