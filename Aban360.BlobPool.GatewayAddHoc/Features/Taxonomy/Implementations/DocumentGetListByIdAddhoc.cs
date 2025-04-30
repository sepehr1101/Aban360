using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Implementations
{
    internal sealed class DocumentGetListByIdAddhoc : IDocumentGetListByIdAddhoc
    {
        private readonly IDocumentGetListByIdHandler _documentGetListByIdHandler;
        public DocumentGetListByIdAddhoc(IDocumentGetListByIdHandler documentGetListByIdHandler)
        {
            _documentGetListByIdHandler = documentGetListByIdHandler;
            _documentGetListByIdHandler.NotNull(nameof(documentGetListByIdHandler));
        }

        public async Task<ICollection<Document>> Handle(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            var result = await _documentGetListByIdHandler.Handle(ids, cancellationToken);
            return result;
        }
    }
}
