using Aban360.ClaimPool.Domain.Features._Base.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities
{
    [Table(nameof(IndividualDiscountType))]
    public class IndividualDiscountType : IndividualDiscountTypeBase
    {
        public virtual Individual Individual { get; set; }
    }
}
