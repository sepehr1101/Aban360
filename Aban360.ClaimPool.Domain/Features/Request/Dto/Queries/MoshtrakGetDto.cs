namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record MoshtrakGetDto
    {
        public int ZoneId { get; set; }
        public int? CustomerNumber { get; set; }
        public string? NationalCode { get; set; }
        public int? TrackNumber { get; set; }
        public MoshtrakGetDto(int zoneId, int? customerNumber, string? nationalCode, int? trackNumber)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            NationalCode = nationalCode;
            TrackNumber = trackNumber;
        }
        public MoshtrakGetDto()
        {
        }
    }
}
