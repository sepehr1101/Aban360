namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BillInstallmentOutputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public int Barge { get; set; }
        public string RegisterDateJalali { get; set; }
        public string DeadLineDateJalali { get; set; }
        public long Payable { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public int QueueNumber { get; set; }
        public string QueueNumberTitle { get; set; }

    }
}
