using Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.DMS.Entities;
using Aban360.ClaimPool.Persistence.Features.DMS.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Create.Implementations
{
    internal sealed class DocumentEntityCreateHandler : IDocumentEntityCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentEntityCommandService _documentEntityCommandService;
        public DocumentEntityCreateHandler(
            IMapper mapper,
            IDocumentEntityCommandService documentEntityCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentEntityCommandService = documentEntityCommandService;
            _documentEntityCommandService.NotNull(nameof(_documentEntityCommandService));
        }

        public async Task Handle(DocumentEntityCreateDto createDto, CancellationToken cancellationToken)
        {
            var documentEntity = _mapper.Map<DocumentEntity>(createDto);
            await _documentEntityCommandService.Add(documentEntity);
        }
    }
}
