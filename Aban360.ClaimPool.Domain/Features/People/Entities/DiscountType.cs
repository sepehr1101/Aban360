using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities
{
    [Table(nameof(DiscountType))]
    public class DiscountType
    {
        public DiscountTypeEnum Id { get; set; }
        public string Title { get; set; }
    }
}
