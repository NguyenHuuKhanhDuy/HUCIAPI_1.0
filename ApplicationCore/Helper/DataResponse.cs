using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Helper
{
    public class DataResponse
    {
        public Object? data { get; set; }
        public string massage { get; set; }
        public int status { get; set; }
        public DataResponse(Object data, string massage, int status)
        {
            this.data = data;
            this.massage = massage;
            this.status = status;
        }
    }
}
