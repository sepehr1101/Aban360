using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries
{
    public record FlowMeterReportGetDto
    {
        [JsonRequired]
        public FlowMeterReportHeader ReportHeader { get; set; } 

        // The provider has not supplied a populated sample for this report type yet.
        [JsonRequired]
        public ICollection<JsonElement> ReadAllReportModelList { get; set; } = new List<JsonElement>();
        [JsonRequired]
        public ICollection<UltraSonicReadingGetDto> UltraSonicReadAllReportModelList { get; set; } = new List<UltraSonicReadingGetDto>();
    }
    public record FlowMeterReportHeader
    {
        public string ReportDateJalali { get; set; } = default!;
        public string BillId { get; set; } = default!;
        public string Firstname { get; set; }= default!;
        public string Surname { get; set; }=default!;
        public string UsageTitle { get; set; } = default!;
        public string Address { get; set; } = default!;
    }
}
