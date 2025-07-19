using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts
{
    public interface IServerReportsGetAllService
    {
        Task<IEnumerable<ServerReportsGetDto>> Get(Guid userId);
    }
}
