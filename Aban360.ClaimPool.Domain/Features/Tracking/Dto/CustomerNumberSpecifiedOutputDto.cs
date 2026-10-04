using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record CustomerNumberSpecifiedOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }

        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public IEnumerable<NumericDictionary> ServiceSelected { get; set; }
    }
}