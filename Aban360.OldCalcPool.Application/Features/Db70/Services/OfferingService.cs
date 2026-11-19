using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Db70.Services;
public interface IOfferingService
{
    Task<NumericDictionary> GetOfferingAsync(SearchByIdInput input, CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class OfferingService : IOfferingService
{
    private readonly IOfferingQueryService _offeringQueryService;
    public OfferingService(IOfferingQueryService offeringQueryService)
    {
        _offeringQueryService = offeringQueryService;
        _offeringQueryService.NotNull(nameof(offeringQueryService));
    }

    public async Task<NumericDictionary> GetOfferingAsync(SearchByIdInput input, CancellationToken cancellationToken)
    {
        NumericDictionary result = await _offeringQueryService.Get(input);
        return result;
    }

    public async Task<IEnumerable<NumericDictionary>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<NumericDictionary> result = await _offeringQueryService.Get();
        return result;
    }
}
