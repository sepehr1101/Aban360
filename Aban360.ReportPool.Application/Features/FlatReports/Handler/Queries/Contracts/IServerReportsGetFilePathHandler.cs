using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;

namespace Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Contracts
{
    public interface IServerReportsGetFilePathHandler
    {
        Task<ServerReportsGetFilePathDto> Handle(Guid id, CancellationToken cancellationToken);
    }
}
