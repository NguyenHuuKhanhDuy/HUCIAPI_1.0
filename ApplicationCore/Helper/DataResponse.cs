using System.Text.Json;

namespace ApplicationCore.Helper
{
    public class DataResponse
    {
        public Object? data { get; set; }
        public string message { get; set; }
        public int status { get; set; }
        public DataResponse(Object data, string massage, int status)
        {
            this.data = data;
            this.message = massage;
            this.status = status;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
