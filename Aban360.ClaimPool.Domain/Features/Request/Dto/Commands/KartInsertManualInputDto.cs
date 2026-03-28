using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record KartInsertManualInputDto
    {
        public int TrackNumber { get; set; }
        public int ServiceGroupId { get; set; }
        public long Amount { get; set; }
        public KartCategoryTypeEnum CategoryType{ get; set; }
    }
}
