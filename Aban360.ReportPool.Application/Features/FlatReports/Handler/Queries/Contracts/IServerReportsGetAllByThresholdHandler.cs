using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;

namespace Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Contracts
{
    public interface IServerReportsGetAllByThresholdHandler
    {
        Task<IEnumerable<ServerReportsGetAllDto>> Handle(Guid userId, int thresholdS, CancellationToken cancellationToken);
    }
}
