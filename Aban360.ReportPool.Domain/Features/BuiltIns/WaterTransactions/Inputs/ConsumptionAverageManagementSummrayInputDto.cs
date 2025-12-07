namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionAverageManagementSummrayInputDto
    {
        public ICollection<int> ZoneIds { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public bool IsDomestic { get; set; }
        public bool IsNet { get; set; }

    }
}
