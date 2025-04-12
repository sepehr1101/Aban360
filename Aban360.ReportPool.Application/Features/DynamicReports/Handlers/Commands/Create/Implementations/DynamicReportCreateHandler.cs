using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Commands.Create.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Commands;
using Aban360.ReportPool.Domain.Features.DynamicReports.Entities;
using Aban360.ReportPool.Persistence.Features.DynamicReports.Commands.Contracts;
using AutoMapper;

namespace Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Commands.Create.Implementations
{
    internal sealed class DynamicReportCreateHandler : IDynamicReportCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDynamicReportCommandService _dynamicReportCommandService;
        public DynamicReportCreateHandler(
            IMapper mapper,
            IDynamicReportCommandService dynamicReportCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _dynamicReportCommandService = dynamicReportCommandService;
            _dynamicReportCommandService.NotNull(nameof(_dynamicReportCommandService));
        }

        public async Task Handle(DynamicReportCreateDto createDto, CancellationToken cancellationToken)
        {
            var dynamicReport = _mapper.Map<DynamicReport>(createDto);
            await _dynamicReportCommandService.Add(dynamicReport);
        }
    }
}
