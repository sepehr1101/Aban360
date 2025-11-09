namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BedBesDataInfoOutptuDto
    {
        public string BillId { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public string DateBed { get; set; }
        public int CounterStateCode { get; set; }
        public long BodySerial { get; set; }
    }
}
