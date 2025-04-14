using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features._Base.Entities
{
    [Table(nameof(IndividualDiscountTypeBase))]
    public class IndividualDiscountTypeBase : IHashableEntity
    {
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public DiscountTypeEnum DiscountTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpireDate { get; set; }

        public virtual DiscountType DiscountType { get; set; }

    }
}
