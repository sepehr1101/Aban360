using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IConnectDisconnectQueryService
    {
        Task<IEnumerable<ConnectDisconnectGetDto>> Get();
        Task<ConnectDisconnectGetDto> Get(long id,int typeId);
    }
}
