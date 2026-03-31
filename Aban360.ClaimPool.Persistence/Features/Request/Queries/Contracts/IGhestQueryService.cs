using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IGhestQueryService
    {
        Task<IEnumerable<InstallmentRequestDataOutputDto>> Get(string stringTrackNumber, int zoneId);
        Task<InstallmentRequestDataOutputDto> Get(int id, int zoneId);
    }
}
