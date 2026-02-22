using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterChangeInfoOutputDto
    {
        public int CustomerNumber { get; set; }
        public int MeterNumber { get; set; }
        public string MeterChangeDateJalali { get; set; }
        public string RegisterDateJalali { get; set; } 
        public string? BodySerial { get; set; }
        public int ChangeCauseId { get; set; }
        public string ChangeCauseTitle { get; set; }
    }
}
