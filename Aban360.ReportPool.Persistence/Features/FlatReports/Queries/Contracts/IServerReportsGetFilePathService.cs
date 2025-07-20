using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts
{
    public interface IServerReportsGetFilePathService
    {
        Task<ServerReportsGetFilePathDto> Get(Guid id);
    }
}