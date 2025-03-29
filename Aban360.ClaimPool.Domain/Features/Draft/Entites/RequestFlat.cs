using Aban360.ClaimPool.Domain.Features._Base;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestFlat:FlatBase
    {
        public virtual RequestEstate RequestEstate { get; set; }
    }
}
