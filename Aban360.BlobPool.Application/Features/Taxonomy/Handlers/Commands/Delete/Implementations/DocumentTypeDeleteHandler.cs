using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Implementations
{
    internal sealed class DocumentTypeDeleteHandler : IDocumentTypeDeleteHandler
    {
        private readonly IDocumentTypeCommandService _documentTypeCommandService;
        private readonly IDocumentTypeQueryService _documentTypeQueryService;
        public DocumentTypeDeleteHandler(
            IDocumentTypeCommandService documentTypeCommandService,
            IDocumentTypeQueryService documentTypeQueryService)
        {
            _documentTypeCommandService = documentTypeCommandService;
            _documentTypeCommandService.NotNull(nameof(_documentTypeCommandService));

            _documentTypeQueryService = documentTypeQueryService;
            _documentTypeQueryService.NotNull(nameof(_documentTypeQueryService));
        }

        public async Task Handle(DocumentTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var documentType = await _documentTypeQueryService.Get(deleteDto.Id);
            _documentTypeCommandService.Remove(documentType);
        }
    }
}
