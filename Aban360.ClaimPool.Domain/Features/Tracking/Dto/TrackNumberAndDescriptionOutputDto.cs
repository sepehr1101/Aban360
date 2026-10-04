using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackNumberAndDescriptionOutputDto
    {
        public int TrackNumber { get; set; }
        public string BillId { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public string? Description { get; set; }
        public IEnumerable<NumericDictionary> ServiceSelected { get; set; }
    }
}