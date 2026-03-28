using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IKartQueryService
    {
        Task<IEnumerable<CalculationRequestDisplayDataOutputDto>> Get(string stringTrackNumber, int zoneId);
    }
}
