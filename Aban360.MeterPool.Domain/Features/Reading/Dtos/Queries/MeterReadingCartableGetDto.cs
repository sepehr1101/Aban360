namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record MeterReadingCartableGetDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public IEnumerable<int> ZoneIds { get; set; }
        public MeterReadingCartableGetDto(string? fromReadingNumber, string? toReadingNumber, IEnumerable<int> zoneIds)
        {
            FromReadingNumber = fromReadingNumber;
            ToReadingNumber = toReadingNumber;
            ZoneIds = zoneIds;
        }
        public MeterReadingCartableGetDto()
        {
        }
    }
}
