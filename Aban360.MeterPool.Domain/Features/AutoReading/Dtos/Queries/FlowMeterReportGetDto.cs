using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries
{
    public record FlowMeterReportGetDto
    {
        // The provider has not supplied a populated sample for this report type yet.
        [JsonRequired]
        public ICollection<JsonElement> ReadAllReportModelList { get; set; } = new List<JsonElement>();
        [JsonRequired]
        public ICollection<UltraSonicReadingGetDto> UltraSonicReadAllReportModelList { get; set; } = new List<UltraSonicReadingGetDto>();
    }
}
