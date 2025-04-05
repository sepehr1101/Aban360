using Aban360.ClaimPool.Domain.Features._Base.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    [Table(nameof(RequestFlat))]
    public class RequestFlat:FlatBase
    {
        [ForeignKey(nameof(EstateId))]
        public virtual RequestEstate RequestEstate{ get; set; }
    }
}
