using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public abstract class BaseServices
    {
        public string FormatDateTime(DateTime dt, string format)
        {
            return dt.ToString(format);
        }
    }
}
