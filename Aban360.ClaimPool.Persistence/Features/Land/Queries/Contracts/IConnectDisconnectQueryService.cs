using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IConnectDisconnectQueryService
    {
        Task<IEnumerable<ConnectDisconnectGetDto>> Get();
        Task<IEnumerable<ConnectDisconnectDataOutputDto>> Get(int zoneId, bool isNoResult, bool isNoRemoved);
        Task<ConnectDisconnectGetDto> Get(long id, int? typeId);
        Task<ConnectDisconnectGetDto?> Get(ConnectDisconnectGetWithConditionDto inputDto);
        Task<IEnumerable<ConnectDisconnectMainDataOutputDto>> Get(ConnectDisconnectMainInputDto inputDto);
        Task<IEnumerable<ConnectDisconnectMainByCompanyDataOutputDto>> GetWithCompany(ConnectDisconnectMainInputDto inputDto);
        Task<IEnumerable<ConnectDisconnectDetailDataOutputDto>> Get(ConnectDisconnectDetailInputDto inputDto);
        Task<IEnumerable<ConnectDisconnectDetailByCompanyDataOutputDto>> GetWithCompany(ConnectDisconnectDetailInputDto inputDto);
        Task<IEnumerable<ConnectDisconnectVeryDetailDataOutputDto>> Get(ConnectDisconnectVeryDetailInputDto input);
    }
}
