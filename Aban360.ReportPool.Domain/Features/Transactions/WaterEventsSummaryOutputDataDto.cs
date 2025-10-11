namespace Aban360.ReportPool.Domain.Features.Transactions
{
    public record WaterEventsSummaryOutputDataDto
    {
        public long Id { get; set; }
        public short CommercialUnit { get; set; }
        public short DomesticUnit { get; set; }
        public short OtherUnit { get; set; }
        public short EmptyUnit { get; set; }
        public short HouseholderNumber { get; set; }
        public short ContractualCapacity { get; set; }
        public short UsageSellId { get; set; }
        public short UsageConsumptionId { get; set; }
        public string UsageSellTitle { get; set; } = default!;
        public string UsageConsumptionTitle { get; set; } = default!;
        public string? ReadingNumber { get; set; }
        public double Consumption { get; set; }
        public double ConsumptionAverage { get; set; }
        public string Date { get; set; }
        public string? ReadingDate { get; set; }
        public long? DebtAmount { get; set; }
        public long CreditAmount { get; set; }
        public long Remained { get; set; }
        public string Description { get; set; } = default!;
        public string? BankTitle { get; set; }
        public int? BankCode { get; set; }

        public string BillId { get; set; } = default!;
        public int? PreviousMeterNumber { get; set; }//
        public int? NextMeterNumber { get; set; }
        public string? PreviousMeterDate { get; set; }//
        public string? CurrentMeterDate { get; set; }//
        public string RegisterDate { get; set; } = default!;
        public string? PayDateJalali { get; set; }
        public string? EventDateJalali{ get; set; }
        public int TypeCode { get; set; }
    }
}
