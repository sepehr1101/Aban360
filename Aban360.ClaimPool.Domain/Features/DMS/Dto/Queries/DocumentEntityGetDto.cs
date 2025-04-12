using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.DMS.Dto.Queries
{
    public record DocumentEntityGetDto
    {
        public long Id { get; set; }
        public Guid DocumentId { get; set; }
        public long TableId { get; set; }
        public RelationEntityEnum RelationEntityId { get; set; }
    }
}
