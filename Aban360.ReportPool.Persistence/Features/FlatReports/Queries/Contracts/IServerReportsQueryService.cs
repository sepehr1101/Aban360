using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts
{
    public interface IServerReportsQueryService
    {
        Task<ServerReportsGetByIdDto?> Get(string jsonInput);
        Task<ServerReportsGetByIdDto?> Get(Guid userId);
    }
}
