using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Implementations
{
    internal sealed class DocumentGetListByIdHandler : IDocumentGetListByIdHandler
    {
        private readonly IDocumentQueryService _documentQueryService;
        public DocumentGetListByIdHandler(IDocumentQueryService documentQueryService)
        {
            _documentQueryService = documentQueryService;
            _documentQueryService.NotNull(nameof(documentQueryService));
        }

        public async Task<ICollection<Document>> Handle(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var document = await _documentQueryService.Get(ids);
            return document;
        }
    }
}
