using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Commands.Update.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Commands;
using Aban360.ReportPool.Persistence.Features.DynamicReports.Queries.Contracts;
using AutoMapper;

namespace Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Commands.Update.Implementations
{
    internal sealed class DynamicReportUpdateHandler : IDynamicReportUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDynamicReportQueryService _dynamicReportQueryService;
        public DynamicReportUpdateHandler(
            IMapper mapper,
            IDynamicReportQueryService dynamicReportQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _dynamicReportQueryService = dynamicReportQueryService;
            _dynamicReportQueryService.NotNull(nameof(_dynamicReportQueryService));
        }

        public async Task Handle(DynamicReportUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var dynamicReport = await _dynamicReportQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, dynamicReport);
        }
    }
}
