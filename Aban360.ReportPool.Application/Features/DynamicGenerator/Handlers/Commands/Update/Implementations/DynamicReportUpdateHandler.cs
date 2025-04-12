using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Update.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;
using Aban360.ReportPool.Persistence.Features.DynamicGenerator.Queries.Contracts;
using AutoMapper;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Update.Implementations
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

        public async Task Handle(IAppUser currentUser, DynamicReportUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var dynamicReport = await _dynamicReportQueryService.Get(updateDto.Id);
            dynamicReport.UserName = currentUser.Username;

            dynamicReport.ValidFrom = DateTime.Now;
            dynamicReport.InsertLogInfo = "insertLogInfo";
            dynamicReport.Hash = "hash";
            _mapper.Map(updateDto, dynamicReport);

        }
    }
}
