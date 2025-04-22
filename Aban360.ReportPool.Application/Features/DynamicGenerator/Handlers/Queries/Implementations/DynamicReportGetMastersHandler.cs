using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries;
using Aban360.ReportPool.Persistence.Features.DynamicGenerator.Queries.Contracts;
using AutoMapper;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Implementations
{
    internal sealed class DynamicReportGetMastersHandler : IDynamicReportGetMastersHandler
    {
        private readonly IMapper _mapper;
        private readonly IDynamicReportQueryService _dynamicReportQueryService;
        public DynamicReportGetMastersHandler(
            IMapper mapper,
            IDynamicReportQueryService dynamicReportQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _dynamicReportQueryService = dynamicReportQueryService;
            _dynamicReportQueryService.NotNull(nameof(_dynamicReportQueryService));
        }

        public async Task<ICollection<DynamicReportMasterDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<DynamicReportMasterDto> dynamicReportMasters = await _dynamicReportQueryService.GetMasters();
            return dynamicReportMasters;
        }
    }
}
