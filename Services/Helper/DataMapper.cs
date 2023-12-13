using Newtonsoft.Json;

namespace Services.Helper
{
    public static class DataMapper
    {
        public static List<TOut> MapList<TIn, TOut>(List<TIn> sources)
        {
            var result = new List<TOut>();

            foreach (var source in sources)
            {
                result.Add((TOut)Map<TIn, TOut>(source));
            }

            return result;
        }

        public static TOut Map<TIn, TOut>(TIn source)
        {
            string json = JsonConvert.SerializeObject(
                source,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
                );
            return (TOut)JsonConvert.DeserializeObject<TOut>(json);
        }
    }
}
