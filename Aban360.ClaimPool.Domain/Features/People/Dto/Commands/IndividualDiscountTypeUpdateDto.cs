using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record IndividualDiscountTypeUpdateDto
    {
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public DiscountTypeEnum DiscountTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
