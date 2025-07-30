namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record ZaribInputDto
    {
        public int ZoneId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get;set; }
        public ZaribInputDto(int _zoneId,string _fromDate,string _toDate)
        {
            ZoneId= _zoneId;
            FromDateJalali= _fromDate;
            ToDateJalali= _toDate;
        }
    }
}
