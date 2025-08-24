using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts
{
    public interface IServerReportsGetAllByThresholdService
    {
        Task<IEnumerable<ServerReportsGetAllDto>> Get(Guid userId, int threshold);
    }
}
