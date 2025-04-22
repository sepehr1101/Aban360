using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Update.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;
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

        public async Task Handle(IAppUser currentUser, int id, string name, string reportTemplateJson)
        {
            DynamicReport dynamicReport = await _dynamicReportQueryService.Get(id);
            dynamicReport.UserId=currentUser.UserId;
            dynamicReport.UserDisplayName = currentUser.FullName;
            dynamicReport.InsertLogInfo = "insertLogInfo";
            dynamicReport.Version=dynamicReport.Version+1;
            dynamicReport.Name=name;
            dynamicReport.ReportTemplateJson=reportTemplateJson;
        }
    }
}
