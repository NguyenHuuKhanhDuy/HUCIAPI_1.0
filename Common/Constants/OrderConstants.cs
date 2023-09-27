namespace Common.Constants
{
    public class OrderConstants
    {
        public const string INVALID_CUSTOMER_ID = "ID khách hàng không hợp lệ";
        public const string INVALID_TOTAL_ORDER = "Tổng đơn hàng không hợp lệ";
        public const string INVALID_VOUCHER_ID = "ID voucher không hợp lệ";
        public const string INVALID_ORDER_DISCOUNT = "Chiết khấu đơn hàng không hợp lệ";
        public const string INVALID_ORDER_STATUS_ID = "ID trạng thái đơn hàng không hợp lệ";
        public const string INVALID_PAYMENT_STATUS_ID = "ID trạng thái thanh toán không hợp lệ";
        public const string INVALID_SHIPPING_STATUS_ID = "ID trạng thái vận chuyển không hợp lệ";
        public const string INVALID_SHIPPING_METHOD_ID = "ID phương thức vận chuyển không hợp lệ";
        public const string INVALID_SOURCE_ORDER_ID = "ID đơn hàng gốc không hợp lệ";
        public const string INVALID_PAYMENT_METHOD_ID = "ID phương thức thanh toán không hợp lệ";

        public const string CUSTOMER_NOT_EXISTS = "Khách hàng không tồn tại";
        public const string ORDER_NOT_EXISTS = "Đơn hàng không tồn tại";
        public const string VOUCHER_NOT_EXISTS = "Voucher không tồn tại";
        public const string ORDER_STATUS_NOT_EXISTS = "Trạng thái đơn hàng không tồn tại";
        public const string PAYMENT_STATUS_NOT_EXISTS = "Trạng thái thanh toán không tồn tại";
        public const string SHIPPING_STATUS_NOT_EXISTS = "Trạng thái vận chuyển không tồn tại";
        public const string SHIPPING_METHOD_NOT_EXISTS = "Phương thức vận chuyển không tồn tại";
        public const string USER_CREATE_NOT_EXISTS = "Người tạo không tồn tại";
        public const string SOURCE_ORDER_NOT_EXISTS = "Đơn hàng gốc không tồn tại";
        public const string PAYMENT_METHOD_NOT_EXISTS = "Phương thức thanh toán không tồn tại";
        public const string PROVINCE_NOT_EXISTS = "Tỉnh/Thành phố không tồn tại";
        public const string DISTRICT_NOT_EXISTS = "Quận/Huyện không tồn tại";
        public const string WARD_NOT_EXISTS = "Xã/Phường không tồn tại";

        public const string VOUCHER_EXCEED = "Voucher vượt quá số lần sử dụng";

        public const string PREFIX_ORDER_NUMBER = "DH";

        public const int ORDER_SOURCE_TIKTOK = 1;
        public const int ORDER_SOURCE_TAKE_CARE = 2;
        public const int ORDER_SOURCE_NORMAL = 3;
        public const int PERCENT_COMMISSION_TAKE_CARE = 5;

        public const int OrderStatusWaiting = 1;
        public const int OrderStatusSuccess = 6;
        public const int OrderStatucCompleted = 3;

        public const string OrderStatusDeletedName = "deleted";
        public const string OrderStatusWaitingName = "wait";
        public const string ActionUpSale = "Cập nhật từ \'{0}\' sang \'{1}\'";

    }
}
