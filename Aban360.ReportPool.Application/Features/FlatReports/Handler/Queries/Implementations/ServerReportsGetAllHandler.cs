using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.FlatReports.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Implementations
{
    internal sealed class ServerReportsGetAllHandler : IServerReportsGetAllHandler
    {
        private readonly IServerReportsGetAllService _serverReportsGetAllService;
        public ServerReportsGetAllHandler(IServerReportsGetAllService serverReportsGetAllService)
        {
            _serverReportsGetAllService = serverReportsGetAllService;
            _serverReportsGetAllService.NotNull(nameof(serverReportsGetAllService));
        }

        public async Task<IEnumerable<ServerReportsGetAllDto>> Handle(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _serverReportsGetAllService.Get(userId);
            return result;
        }
    }
}
