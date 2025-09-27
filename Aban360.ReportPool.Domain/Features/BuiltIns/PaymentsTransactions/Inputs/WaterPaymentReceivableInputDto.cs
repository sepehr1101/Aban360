using Newtonsoft.Json;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record WaterPaymentReceivableInputDto
    {
        [JsonIgnore]
        public bool IsZone { get; set; }
        public ICollection<int>? ZoneIds { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string? FromBankId { get; set; }
        public string? ToBankId { get; set; }


    }
}
