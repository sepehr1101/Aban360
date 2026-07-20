namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record RegionAndZoneGetDto
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public RegionAndZoneGetDto(int regionId, string regionTitle, int zoneId, string zoneTitle)
        {
            RegionId = regionId;
            RegionTitle = regionTitle;
            ZoneId = zoneId;
            ZoneTitle = zoneTitle;
        }
        public RegionAndZoneGetDto()
        {
        }
    }
}
