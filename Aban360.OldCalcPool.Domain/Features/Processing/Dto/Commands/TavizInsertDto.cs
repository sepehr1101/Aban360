using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record TavizInsertDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int Operator { get; set; }
        public int MeterNumber { get; set; }
        public string MeterChangeDateJalali { get; set; }
        public string RegisterDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string BodySerial { get; set; }
        public int ChangeCauseId { get; set; }
        public int UsageId { get; set; }
        public int MeterDiameterId { get; set; }
    }
}
