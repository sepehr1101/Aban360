using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Implementations
{
    internal sealed class ServerReportsGetAllByThresholdHandler : IServerReportsGetAllByThresholdHandler
    {
        private readonly IServerReportsGetAllByThresholdService _serverReportsGetAllByThresholdService;
        public ServerReportsGetAllByThresholdHandler(IServerReportsGetAllByThresholdService serverReportsGetAllByThresholdService)
        {
            _serverReportsGetAllByThresholdService = serverReportsGetAllByThresholdService;
            _serverReportsGetAllByThresholdService.NotNull(nameof(serverReportsGetAllByThresholdService));
        }

        public async Task<IEnumerable<ServerReportsGetAllDto>> Handle(Guid userId,int threshold, CancellationToken cancellationToken)
        {
            var result = await _serverReportsGetAllByThresholdService.Get(userId,threshold);
            return result;
        }
    }
}
