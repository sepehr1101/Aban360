using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IKitCodeDictionaryGetHandler
    {
        ICollection<NumericDictionary> Handle(CancellationToken cancellationToken);
    }
}
