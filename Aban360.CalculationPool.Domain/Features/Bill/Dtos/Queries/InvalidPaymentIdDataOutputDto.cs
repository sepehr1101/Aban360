namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record InvalidPaymentIdDataOutputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public long Baha { get; set; }
        public long Jam { get; set; }
        public long Payable { get; set; }
        public string PaymentId { get; set; }
        public string BillId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public int CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }
    }
}
