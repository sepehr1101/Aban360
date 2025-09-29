using System.Text.Json.Serialization;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record ServiceLinkPaymentReceivableInputDto
    {
        [JsonIgnore]
        public bool IsZone { get; set; }
        public ICollection<int>? ZoneIds { get; set; }
        public ICollection<int>? UsageIds { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }


    }
}
