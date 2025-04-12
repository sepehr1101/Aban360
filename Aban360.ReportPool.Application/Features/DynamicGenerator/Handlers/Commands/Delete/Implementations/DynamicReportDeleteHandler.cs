using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Delete.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;
using Aban360.ReportPool.Persistence.Features.DynamicGenerator.Commands.Contracts;
using Aban360.ReportPool.Persistence.Features.DynamicGenerator.Queries.Contracts;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Delete.Implementations
{
    internal sealed class DynamicReportDeleteHandler : IDynamicReportDeleteHandler
    {
        private readonly IDynamicReportCommandService _dynamicReportCommandService;
        private readonly IDynamicReportQueryService _dynamicReportQueryService;
        public DynamicReportDeleteHandler(
            IDynamicReportCommandService dynamicReportCommandService,
            IDynamicReportQueryService dynamicReportQueryService)
        {
            _dynamicReportCommandService = dynamicReportCommandService;
            _dynamicReportCommandService.NotNull(nameof(_dynamicReportCommandService));

            _dynamicReportQueryService = dynamicReportQueryService;
            _dynamicReportQueryService.NotNull(nameof(_dynamicReportQueryService));
        }

        public async Task Handle(DynamicReportDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var dynamicReport = await _dynamicReportQueryService.Get(deleteDto.Id);
            await _dynamicReportCommandService.Remove(dynamicReport);
        }
    }
}
