using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Aban360.BlobPool.Application.Features.Taxonomy.Services;

namespace Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Implementations
{
    internal sealed class DocumentCommandAddhoc : IDocumentCommandAddhoc
    {
        private readonly IDocumentService _documentService;
        public DocumentCommandAddhoc(IDocumentService documentService)
        {
            _documentService = documentService;
            _documentService.NotNull(nameof(documentService));
        }

        public async Task<Guid> Handle(IFormFile file, string description, short documentTypeId, CancellationToken cancellationToken)
        {
            //DocumentCreateDto createDto = new DocumentCreateDto()
            //{
            //    Description = description,
            //    DocumentFile = file,
            //    DocumentTypeId= documentTypeId
            //};
            var documentId = await _documentService.CreateAsync(file, documentTypeId, description, cancellationToken);
            return documentId;
        }
    }
}
