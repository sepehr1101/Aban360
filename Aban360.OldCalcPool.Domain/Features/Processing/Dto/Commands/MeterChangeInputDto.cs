namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record MeterChangeInputDto
    {
        public string BillId { get; set; }
        public int MeterNumber { get; set; }
        public string MeterChangeDateJalali { get; set; }
        public string BodySerial { get; set; }
        public int ChangeCauseId { get; set; }
    }
}
