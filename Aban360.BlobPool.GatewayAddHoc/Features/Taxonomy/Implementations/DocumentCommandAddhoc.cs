using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Implementations
{
    internal sealed class DocumentCommandAddhoc : IDocumentCommandAddhoc
    {
        private readonly IDocumentCreateHandler _documentCreateHandler;
        public DocumentCommandAddhoc(IDocumentCreateHandler documentCreateHandler)
        {
            _documentCreateHandler = documentCreateHandler;
            _documentCreateHandler.NotNull(nameof(documentCreateHandler));
        }

        public async Task<Guid> Handle(IFormFile file, string description,short documentTypeId, CancellationToken cancellationToken)
        {
            DocumentCreateDto createDto = new DocumentCreateDto()
            {
                Description = description,
                document = file,
                DocumentTypeId= documentTypeId
            };
            var documentId = await _documentCreateHandler.Handle(createDto, cancellationToken);
            return documentId;
        }
    }
}
