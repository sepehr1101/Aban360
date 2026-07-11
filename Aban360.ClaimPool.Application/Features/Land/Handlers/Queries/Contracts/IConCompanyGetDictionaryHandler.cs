using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IConCompanyGetDictionaryHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken);
    }
}
