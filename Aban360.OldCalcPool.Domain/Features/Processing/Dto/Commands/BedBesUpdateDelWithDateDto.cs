namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BedBesUpdateDelWithDateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public bool Del { get; set; }
        public BedBesUpdateDelWithDateDto(int zoneId, int customerNumber, bool del,string fromDateJalali,string toDateJalali)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            Del = del;
            FromDateJalali = fromDateJalali;
            ToDateJalali= toDateJalali;
        }
    }
}
