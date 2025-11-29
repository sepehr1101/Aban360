namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterComparisonBatchDataOutputDto
    {
        public string ZoneTitle { get; set; } = default!;
        public string BillId { get; set; } = default!;

        public string PreviousDateJalali { get; set; } = default!;
        public string CurrentDateJalali { get; set; } = default!;

        public int PreviousMeterNumber { get; set; }
        public int CurrentMeterNumber { get; set; }

        public double PreviousAmount { get; set; }
        public double CurrentAmount { get; set; }

        public double PreviousDiscountAmount { get; set; }
        public double CurrentDiscountAmount { get; set; }

        public bool IsChecked{ get; set; }
        public double ComparisonAmount { get; set; }

        public int UsageId { get; set; }
        public int BranchId { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int EmptyUnit { get; set; }
        public int ContractualCapacity { get; set; }
        public int VirtualCategoryId { get; set; }
        public bool IsSpecial { get; set; }
    }
}
