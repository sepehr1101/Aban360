using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Create.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;
using Aban360.ReportPool.Persistence.Features.DynamicGenerator.Commands.Contracts;
using AutoMapper;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Create.Implementations
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
            dynamicReport.ValidFrom=DateTime.Now;
            dynamicReport.InsertLogInfo = "insertLogInfo";
            dynamicReport.Hash = "hash";
            await _dynamicReportCommandService.Add(dynamicReport);
        }
    }
}
