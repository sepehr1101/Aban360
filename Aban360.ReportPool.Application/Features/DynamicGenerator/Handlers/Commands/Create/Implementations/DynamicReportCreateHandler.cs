using Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts;
using Aban360.Common.ApplicationUser;
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
        private readonly IDocumentCommandAddhoc _documentCommandAddhoc;
        public DynamicReportCreateHandler(
            IMapper mapper,
            IDynamicReportCommandService dynamicReportCommandService,
            IDocumentCommandAddhoc documentCommandAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _dynamicReportCommandService = dynamicReportCommandService;
            _dynamicReportCommandService.NotNull(nameof(_dynamicReportCommandService));

            _documentCommandAddhoc = documentCommandAddhoc;
            _documentCommandAddhoc.NotNull(nameof(_documentCommandAddhoc));
        }

        public async Task Handle(IAppUser currentUser,DynamicReportCreateDto createDto, CancellationToken cancellationToken)
        {
            var dynamicReport = _mapper.Map<DynamicReport>(createDto);
            var documentId = await _documentCommandAddhoc.Handle(createDto.Document, createDto.Description,createDto.DocumentTypeId, cancellationToken);

            dynamicReport.UserName = currentUser.Username;
            dynamicReport.DocumentId=documentId;
            dynamicReport.ValidFrom=DateTime.Now;
            dynamicReport.InsertLogInfo = "insertLogInfo";
            dynamicReport.Hash = "hash";
            await _dynamicReportCommandService.Add(dynamicReport);
        }
    }
}
