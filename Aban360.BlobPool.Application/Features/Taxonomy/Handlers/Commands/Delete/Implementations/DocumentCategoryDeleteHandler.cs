using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Implementations
{
    internal sealed class DocumentCategoryDeleteHandler : IDocumentCategoryDeleteHandler
    {
        private readonly IDocumentCategoryCommandService _documentCategoryCommandService;
        private readonly IDocumentCategoryQueryService _documentCategoryQueryService;
        public DocumentCategoryDeleteHandler(
            IDocumentCategoryCommandService documentCategoryCommandService,
            IDocumentCategoryQueryService documentCategoryQueryService)
        {
            _documentCategoryCommandService = documentCategoryCommandService;
            _documentCategoryCommandService.NotNull(nameof(_documentCategoryCommandService));

            _documentCategoryQueryService = documentCategoryQueryService;
            _documentCategoryQueryService.NotNull(nameof(_documentCategoryQueryService));
        }

        public async Task Handle(DocumentCategoryDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var documentCategory = await _documentCategoryQueryService.Get(deleteDto.Id);
            _documentCategoryCommandService.Remove(documentCategory);
        }
    }
}
