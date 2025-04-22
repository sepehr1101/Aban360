using Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Create.Contracts;
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
            IDynamicReportCommandService dynamicReportCommandService,
            IDocumentCommandAddhoc documentCommandAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _dynamicReportCommandService = dynamicReportCommandService;
            _dynamicReportCommandService.NotNull(nameof(_dynamicReportCommandService));
        }

        public async Task Handle(IAppUser currentUser, string name, string reportTemplateJson)
        {
            DynamicReport dynamicReport = new()
            {
                Version = 1,
                UserDisplayName = currentUser.FullName,
                ReportTemplateJson = reportTemplateJson,
                InsertLogInfo = "insertLogInfo",
                Name=name,
                UserId=currentUser.UserId
            };
            await _dynamicReportCommandService.Add(dynamicReport);
        }
    }
}
