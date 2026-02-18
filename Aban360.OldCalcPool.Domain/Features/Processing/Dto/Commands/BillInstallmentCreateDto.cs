using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BillInstallmentCreateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public int Barge { get; set; }
        public string RegisterDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string DeadLineDateJalali { get; set; }
        public long Payable { get; set; }
        public int UsageId { get; set; }
        public int MeterDiameterId { get; set; }
        public int QueueNumber { get; set; }
        public int Operator { get; set; }
    }
}
