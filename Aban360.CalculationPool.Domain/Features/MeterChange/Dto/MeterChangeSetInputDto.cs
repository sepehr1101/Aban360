namespace Aban360.CalculationPool.Domain.Features.MeterChange.Dto
{
    public record MeterChangeSetInputDto
    {
        public string BillId { get; set; }
        public string BodySerial { get; set; }
        public int ChangeCuaseId { get; set; }
        public string ChangeDateJalali { get; set; }
        public int MeterNumber { get; set; }
    }
}
