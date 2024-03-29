﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public static class BaseConstants
    {
        public const string DELETE = " {XÓA}";

        // Định dạng ngày giờ
        public const string FORMAT_DATETIME_DOB = "dd/MM/yyyy";

        public const string USER_CREATE_NOT_EXIST = "Người tạo không tồn tại";

        public const int INT_DEFAULT = 0;

        public const string LOCATION_NOT_EXIST = "Địa điểm không tồn tại";

        public const string ADMIN_ID = "0A1ECDDB-067D-4302-A3B0-E986BBAFD19F";
        public const bool IsActiveDefault = true;
        public const bool IsDeletedDefault = false;

        public const int PageDefault = 1;
        public const int PageSizeDefault = 20;

        public const string NameDefault = "--";
    }
}
