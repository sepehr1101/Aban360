using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.DynamicReports.Queries.Contracts;
using AutoMapper;

namespace Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Queries.Implementations
{
    internal sealed class DynamicReportGetSingleHandler : IDynamicReportGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IDynamicReportQueryService _dynamicReportQueryService;
        public DynamicReportGetSingleHandler(
            IMapper mapper,
            IDynamicReportQueryService dynamicReportQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _dynamicReportQueryService = dynamicReportQueryService;
            _dynamicReportQueryService.NotNull(nameof(_dynamicReportQueryService));
        }

        public async Task<DynamicReportGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var dynamicReport = await _dynamicReportQueryService.Get(id);
            return _mapper.Map<DynamicReportGetDto>(dynamicReport);
        }
    }
}
