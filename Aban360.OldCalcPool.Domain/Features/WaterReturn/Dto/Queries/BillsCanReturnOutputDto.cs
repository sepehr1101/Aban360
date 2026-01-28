namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record BillsCanReturnOutputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public int Consumption { get; set; }
        public float MonthlyConsumption { get; set; }
        public long Payable { get; set; }
        public long SumItems { get; set; }
        public long Discount { get; set; }
        public int AgentCode { get; set; }

        public string PaymentId { get; set; }
        public string BillId { get; set; }
        public string UsageTitle { get; set; }
        public string BranchTypeTitle { get; set; }
        public float ContractualCapacity { get; set; }
        public int EmptyUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int HouseholdNumber { get; set; }

        public bool IsReturned { get; set; }
        public bool IsDomestic { get; set; }
    }
}
