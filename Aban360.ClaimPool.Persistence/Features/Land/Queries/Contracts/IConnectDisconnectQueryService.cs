using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IConnectDisconnectQueryService
    {
        Task<IEnumerable<ConnectDisconnectGetDto>> Get();
        Task<IEnumerable<ConnectDisconnectDataOutputDto>> Get(int zoneId, bool isNoResult,bool isNoRemoved);
        Task<ConnectDisconnectGetDto> Get(long id, int? typeId);
        Task<ConnectDisconnectGetDto?> Get(ConnectDisconnectGetWithConditionDto inputDto);
    }
}
