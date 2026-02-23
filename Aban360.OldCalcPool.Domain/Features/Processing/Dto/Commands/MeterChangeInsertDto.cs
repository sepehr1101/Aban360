using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record MeterChangeInsertDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int MeterNumber { get; set; }
        public string MeterChangeDateJalali { get; set; }
        public string RegisterDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string BodySerial { get; set; }
        public int ChangeCauseId { get; set; }
        public string ChangeCauseTitle { get; set; }
    }
}
