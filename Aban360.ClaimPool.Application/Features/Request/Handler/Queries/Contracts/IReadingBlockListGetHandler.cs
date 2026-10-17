using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IReadingBlockListGetHandler
    {
        IEnumerable<StringDictionary> Handle(CancellationToken cancellationToken);
    }
}
