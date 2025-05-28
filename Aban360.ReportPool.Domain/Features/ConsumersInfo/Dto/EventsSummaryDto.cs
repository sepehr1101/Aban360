namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record EventsSummaryDto
    {
        public short CommercialUsage { get; set; }
        public short ResidentialUsage { get; set; }
        public short OtherUsage { get; set; }
        public short HouseholderNumber  { get; set; }
        public short EmptyUnit  { get; set; }
        public short ContractualCapacity { get; set; }
        public short  Usage{ get; set; }
        public string ReadingNumber { get; set; }
        public string Consumption { get; set; }
        public string ConsumptionAverage { get; set; }
        public string  Date{ get; set; }
        public string ReadingDate { get; set; }
        public long? DebtAmount{ get; set; }
        public long creditAmount { get; set; }
        public string  Remained{ get; set; }
        public string Description { get; set; }
        public string  BankTitle{ get; set; }


        public string BillId { get; set; } = default!;
        //public long Id { get; set; }
        public int? PreviousMeterNumber { get; set; }//
        public int? NextMeterNumber { get; set; }//
        //public string? Description { get; set; }//
        //public string Style { get; set; } = default!;
        //public long? DebtAmount { get; set; }//
        //public long? CreditAmount { get; set; }//
        public string? PreviousMeterDate { get; set; }//
        public string? CurrentMeterDate { get; set; }//
        public string RegisterDate { get; set; } = default!;//
        //public float? ConsumptionAverage { get; set; }//
        //public string? BankTitle { get; set; }//
    }
}
