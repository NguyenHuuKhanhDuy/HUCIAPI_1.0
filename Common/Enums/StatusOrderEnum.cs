using System.ComponentModel;

namespace Common.Enums
{
    public enum StatusOrderEnum
    {
        [Description("--")]
        Default = 0,
        [Description("Waiting for process")]
        Waiting = 1,
        [Description("Complete")]
        Complete = 2,
    }
}
