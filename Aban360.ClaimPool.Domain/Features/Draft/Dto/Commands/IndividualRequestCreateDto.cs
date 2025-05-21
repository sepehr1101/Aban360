using Aban360.ClaimPool.Domain.Features._Base.Dto;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record IndividualRequestCreateDto : IndividualCommandBaseDto
    {
        public int EstateId { get; set; }//?
        public ICollection<RequestIndividualDiscountTypeCreateDto> IndividualDiscountTypes { get; set; }

    }
}
