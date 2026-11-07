using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Contracts
{
    public interface IServerReportsUpdateService
    {
        Task Update(ServerReportsUpdateDto input, CancellationToken cancellationToken);
        Task Fail(Guid id, CancellationToken cancellationToken);
    }
}
