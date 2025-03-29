using Aban360.ClaimPool.Domain.Features._Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestFlat:FlatBase
    {
        [ForeignKey(nameof(EstateId))]
        public virtual RequestEstate RequestEstate{ get; set; }
    }
}
