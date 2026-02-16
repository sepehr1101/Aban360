namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record RemoveBillDataInputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int Barge { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public int Consumption { get; set; }
        public string PaymentId { get; set; }
        public long AbBahaAmount { get; set; }
        public long FazelabAmount { get; set; }
        public long Baha { get; set; }
        public string BillId { get; set; }
        public string? ToDayDateJalali { get; set; }
        public long Discount { get; set; }
    }
}
