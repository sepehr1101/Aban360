using Newtonsoft.Json;

namespace Aban360.Common.Extensions
{
    public static class JsonOperation
    {       
        public static T Clone<T>(this T source)
        {
            source.NotNull(nameof(source));
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static string Marshal(this object obj)
        {
            obj.NotNull(nameof(obj));
            return JsonConvert.SerializeObject(obj);
        }
       
        public static T Unmarshal<T>(this string jsonString)
        {
            jsonString.NotNull(nameof(jsonString));
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
