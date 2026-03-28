namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record GhestInsertDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string StringTrackNumber { get; set; }
        public int Identify { get; set; }
        public int Cod1 { get; set; }
        public int Cod2 { get; set; }
        public int Cod3 { get; set; }
        public int Barge { get; set; }
        public long Payable { get; set; }
        public int Type { get; set; }
        public int InstallmentNumber { get; set; }
        public string CurrentDateJalali { get; set; }
        public string DueDateJalali { get; set; }
        public string InsertBy { get; set; }
        public string BillId { get; set; }
        public string PaymentId { get; set; }


    }
}
