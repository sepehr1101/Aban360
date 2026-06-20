using Aban360.Common.BaseEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public static async Task<JsonReportId> ExportToJson<THeader, TData>(ReportOutput<THeader, TData> reportOutput, CancellationToken cancellationToken, int fileCode, bool hasLogo=false)
        {
            const string path = @"AppData\Jsons\";
            //const string logoPath = @"AppData\Images\logoBase64.txt";

            reportOutput.NotNull(nameof(reportOutput));
            Guid id = Guid.NewGuid();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            string? logoBase64 = null;           
            var outputObject = JObject.FromObject(reportOutput, JsonSerializer.Create(settings));

            if (hasLogo )//&& Path.Exists(logoPath))
            {
                //logoBase64 = await File.ReadAllTextAsync(logoPath, cancellationToken);
                logoBase64 =await Base64Operation.GetLogoBase64(cancellationToken);
            }
            if (logoBase64 != null && outputObject["reportHeader"] is JObject headerObject)
            {
                headerObject["logoBase64"] = logoBase64;
            }

            string jsonString = outputObject.ToString(Formatting.Indented);//reportOutput.Marshal(settings);
            var fileName = Path.Combine(path, $"{id}.json");
            await File.WriteAllTextAsync(fileName, jsonString, Encoding.UTF8, cancellationToken);
            return new JsonReportId(id, fileCode);
        }
        public static async Task<JsonReportId> ExportToJsonFlat<TFlatData>(TFlatData reportOutput, CancellationToken cancellationToken, int fileCode, bool hasLogo = false)
        {
            const string path = @"AppData\Jsons\";
            //const string logoPath = @"AppData\Images\logoBase64.txt";

            reportOutput.NotNull(nameof(reportOutput));
            Guid id = Guid.NewGuid();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling=NullValueHandling.Include
            };
            string? logoBase64 = null;
            var outputObject = JObject.FromObject(reportOutput, JsonSerializer.Create(settings));
            if (hasLogo)// && Path.Exists(logoPath))
            {
                //logoBase64 = await File.ReadAllTextAsync(logoPath, cancellationToken);
                logoBase64 = await Base64Operation.GetLogoBase64(cancellationToken);
            }
            if (logoBase64 != null && outputObject["reportHeader"] is JObject headerObject)
            {
                headerObject["logoBase64"] = logoBase64;
            }
            //string jsonString = reportOutput.Marshal(settings);
            string jsonString = outputObject.ToString(Formatting.Indented);
            var fileName = Path.Combine(path, $"{id}.json");
            await File.WriteAllTextAsync(fileName, jsonString, Encoding.UTF8, cancellationToken);
            return new JsonReportId(id, fileCode);
        }
    }
}
