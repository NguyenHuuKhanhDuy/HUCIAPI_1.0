namespace Common.Constants
{
    public static class EmployeeConstants
    {
        public const string EMPLOYEE_EMPTY = "Nhân viên trống";
        public const string EMPLOYEE_NOT_EXIST = "Nhân viên không tồn tại";
        public const string INVALID_EMAIL = "Địa chỉ email không hợp lệ";
        public const string INVALID_PHONE = "Số điện thoại không hợp lệ";
        public const string INVALID_BIRTHDAY = "Ngày sinh không hợp lệ (vd: 31/12/2023)";
        public const string INVALID_GENGER = "Giới tính không hợp lệ";
        public const string INVALID_PROVINCE = "Mã tỉnh/thành phố không hợp lệ";
        public const string INVALID_DISTRICT = "Mã quận/huyện không hợp lệ";
        public const string INVALID_SALARY = "Lương không hợp lệ";
        public const string INVALID_SALARY_TYPE = "Loại lương không hợp lệ";
        public const string INVALID_RULE = "Mã phân quyền không hợp lệ";
        public const string INVALID_WARD = "Mã phường/xã không hợp lệ";
        public const string INVALID_USER_CREATE = "Mã người tạo không hợp lệ";
        public const string EXIST_EMAIL = "Email đã tồn tại";
        public const string EXIST_PHONE = "Số điện thoại đã tồn tại";
        public const string EXIST_USERNAME = "Tên đăng nhập đã tồn tại";

        public const string RULE_NOTE_EXIST = "Quy định không tồn tại";
        public const string SALARY_TYPE_NOTE_EXIST = "Loại lương không tồn tại";

        public const string PASS_DEFAULT = "passDefault";

        public const string AdminName = "admin";
        public const int AdminRoleId = 1;
        public const string CanNotRemoveAdmin = "Không thể xóa admin";
        public const string EmployeeDoNotPermission = "Nhân viên không có quyền thao tác";
        public const string SecretKeyResetPasswordAppSetting = "SecretKeyResetPassword";
    }
}
