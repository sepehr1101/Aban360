using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Implementations
{
    internal sealed class DocumentGetContentTypeAddhoc : IDocumentGetContentTypeAddhoc
    {
        private readonly IDocumentGetSingleHandler _documentGetSingleHandler;
        public DocumentGetContentTypeAddhoc(IDocumentGetSingleHandler documentGetSingleHandler)
        {
            _documentGetSingleHandler = documentGetSingleHandler;
            _documentGetSingleHandler.NotNull(nameof(documentGetSingleHandler));
        }

        public async Task<byte[]> Handle(Guid id, CancellationToken cancellationToken)
        {
            var result = await _documentGetSingleHandler.Handle(id, cancellationToken);
            return result.FileContent;
        }
    }
}
