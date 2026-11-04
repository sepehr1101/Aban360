using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentDuplicateInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int DuplicateCheckItem { get; set; }
        public IEnumerable<int> ZoneIds { get; set; }

    }
}
