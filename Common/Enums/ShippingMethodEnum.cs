using System.ComponentModel;

namespace Common.Enums
{
    public enum ShippingMethodEnum
    {
        [Description("--")]
        Default = 0,
        [Description("Tự giao")]
        TuGiao = 1,
        [Description("GHTK")]
        GHTK = 2,
        [Description("EMS")]
        EMS = 3,
        [Description("Bank tranfer")]
        BankTranfer = 4
    }
}
