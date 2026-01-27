namespace Aban360.CalculationPool.Domain.Features.MeterChange.Dto
{
    public record MeterChangeSetInputDto
    {
        public string BillId { get; set; } = default!;
        public string BodySerial { get; set; } = default!;
        public int ChangeCuaseId { get; set; }
        public string ChangeDateJalali { get; set; } = default!;
        public int MeterNumber { get; set; }
    }
}
