using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record WaterIncomeDiscountDetailInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public int? FromConsumption { get; set; }
        public int? ToConsumption { get; set; }

        public double? FromAmount { get; set; }
        public double? ToAmount { get; set; }

        public WaterIncomeAndConsumptionTypeEnum type{ get; set; }
        public WaterIncomeDiscountCauseEnum  DiscountCauseId { get; set; }

        public ICollection<int> ZoneIds { get; set; }

    }
}
