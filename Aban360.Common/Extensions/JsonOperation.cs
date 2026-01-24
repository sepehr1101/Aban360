using Aban360.Common.BaseEntities;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;

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

        public static string Marshal(this object obj, [Optional] JsonSerializerSettings? settings)
        {
            obj.NotNull(nameof(obj));
            settings ??= new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(obj,settings);
        }
       
        public static T Unmarshal<T>(this string jsonString)
        {
            jsonString.NotNull(nameof(jsonString));
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static async Task<JsonReportId> ExportToJson<THeader, TData>(ReportOutput<THeader, TData> reportOutput, CancellationToken cancellationToken, [Optional]int fileCode)
        {
            const string path = @"AppData\Jsons\";
            reportOutput.NotNull(nameof(reportOutput));
            Guid id = Guid.NewGuid();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            string jsonString = reportOutput.Marshal(settings);
            var fileName = Path.Combine(path, $"{id}.json");
            await File.WriteAllTextAsync(fileName, jsonString, Encoding.UTF8, cancellationToken);
            return new JsonReportId(id, fileCode);
        }
        public static async Task<JsonReportId> ExportToJson<TFlatData>(TFlatData reportOutput, CancellationToken cancellationToken, [Optional]int fileCode)
        {
            const string path = @"AppData\Jsons\";
            reportOutput.NotNull(nameof(reportOutput));
            Guid id = Guid.NewGuid();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            string jsonString = reportOutput.Marshal(settings);
            var fileName = Path.Combine(path, $"{id}.json");
            await File.WriteAllTextAsync(fileName, jsonString, Encoding.UTF8, cancellationToken);
            return new JsonReportId(id, fileCode);
        }
    }
}
