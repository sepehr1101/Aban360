using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record RequestIndividualDiscountTypeCreateDto
    {
        //public int IndividualId { get; set; }
        public DiscountTypeEnum DiscountTypeId { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
