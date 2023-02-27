using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public static class StatusCodeConstants
    {
        public const int STATUS_SUCCESS = 200;
        public const int STATUS_EXP_VALIDATE = 400;
        public const int STATUS_EXP_BUSINESS = 4001;
        public const int STATUS_NOT_FOUND = 404;
        public const int STATUS_INTERNAL_SERVER_ERROR = 500;
        public const string STRING_EMPTY = "";
    }
}
