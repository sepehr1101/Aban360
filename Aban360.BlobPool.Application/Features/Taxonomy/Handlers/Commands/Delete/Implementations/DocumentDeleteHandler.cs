using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Implementations
{
    internal sealed class DocumentDeleteHandler : IDocumentDeleteHandler
    {
        private readonly IDocumentCommandService _documentCommandService;
        private readonly IDocumentQueryService _documentQueryService;
        public DocumentDeleteHandler(
            IDocumentCommandService documentCommandService,
            IDocumentQueryService documentQueryService)
        {
            _documentCommandService = documentCommandService;
            _documentCommandService.NotNull(nameof(_documentCommandService));

            _documentQueryService = documentQueryService;
            _documentQueryService.NotNull(nameof(_documentQueryService));
        }

        public async Task Handle(DocumentDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var Document = await _documentQueryService.Get(deleteDto.Id);
            await _documentCommandService.Remove(Document);
        }
    }
}
