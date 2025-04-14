using Aban360.ClaimPool.Domain.Features._Base.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    [Table(nameof(RequestIndividualDiscountType))]
    public class RequestIndividualDiscountType : IndividualDiscountTypeBase
    {
        public virtual RequestIndividual Individual { get; set; }
    }
}
