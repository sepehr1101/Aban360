namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record UnconfirmedBillReturnDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public int MinutesNumber { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int HouseholdCount { get; set; }
        public int Duration { get; set; }
        public float ConsumptionAverage { get; set; }
        public int Consumption { get; set; }
        public int BillCount { get; set; }
        public long Amount { get; set; }
        public long WaterAmount { get; set; }
        public long SewageAmount { get; set; }
        public int ReturnCauseId { get; set; }
        public string ReturnCauseTitle { get; set; }
        public int Operator { get; set; }
    }
}
