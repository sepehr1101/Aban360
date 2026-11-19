using Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts;
using Aban360.Common.Extensions;
using Aban360.BlobPool.Application.Features.Taxonomy.Services;

namespace Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Implementations
{
    internal sealed class DocumentGetContentTypeAddhoc : IDocumentGetContentTypeAddhoc
    {
        private readonly IDocumentService _documentService;
        public DocumentGetContentTypeAddhoc(IDocumentService documentService)
        {
            _documentService = documentService;
            _documentService.NotNull(nameof(documentService));
        }

        public async Task<byte[]> Handle(Guid id, CancellationToken cancellationToken)
        {
            var result = await _documentService.GetSingleAsync(id, cancellationToken);
            return result.FileContent;
        }
    }
}
