using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands
{
    public record DocumentEntityCreateDto
    {
        public Guid DocumentId { get; set; }
        public long TableId { get; set; }
        public RelationEntityEnum RelationEntityId { get; set; }
    }
}
