using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IModifyCauseGetAllHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken);
    }
}
