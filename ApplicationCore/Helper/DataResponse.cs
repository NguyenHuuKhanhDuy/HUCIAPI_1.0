using Newtonsoft.Json;
using System.Text.Json;

namespace ApplicationCore.Helper
{
    public class DataResponse
    {
        public Object? Data { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public DateTime DateTime { get; set; }
        public DataResponse() { }
        public DataResponse(Object data, string massage, int status)
        {
            this.Data = data;
            this.Message = massage;
            this.Status = status;
            this.DateTime = DateTime.UtcNow.AddHours(7);
        }

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}
