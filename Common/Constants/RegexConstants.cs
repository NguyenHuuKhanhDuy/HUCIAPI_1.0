namespace Common.Constants
{
    public static class RegexConstants
    {
        public const string REGEX_EMAIL = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        public const string REGEX_PASSWORD = "Password";
        public const string REGEX_PHONE = @"^(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})$";
        public const string REGEX_BIRTHDAY = @"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$";
        public const string REGEX_GUID = @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$";
    }
}
