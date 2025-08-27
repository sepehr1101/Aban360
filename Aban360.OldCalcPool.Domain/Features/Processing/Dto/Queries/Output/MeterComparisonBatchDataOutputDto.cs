namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterComparisonBatchDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }

        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }

        public int PreviousMeterNumber { get; set; }
        public int CurrentMeterNumber { get; set; }

        public double PreviousAmount { get; set; }
        public double CurrentAmount { get; set; }

        public bool IsChecked{ get; set; }

    }
}
