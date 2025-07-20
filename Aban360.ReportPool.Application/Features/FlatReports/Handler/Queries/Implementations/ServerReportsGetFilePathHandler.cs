using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries
{
    internal sealed class ServerReportsGetFilePathHandler : IServerReportsGetFilePathHandler
    {
        private readonly IServerReportsGetFilePathService _serverReportsGetFilePathService;
        public ServerReportsGetFilePathHandler(IServerReportsGetFilePathService serverReportsGetFilePathService)
        {
            _serverReportsGetFilePathService = serverReportsGetFilePathService;
            _serverReportsGetFilePathService.NotNull(nameof(serverReportsGetFilePathService));
        }

        public async Task<ServerReportsGetFilePathDto> Handle(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _serverReportsGetFilePathService.Get(userId);
            return result;
        }
    }
}
