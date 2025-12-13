namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record ManualBillDataOutputDto
    {
        public string BillId { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public long Amount { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public int CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
    }
}