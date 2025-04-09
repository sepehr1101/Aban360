using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Implementations
{
    internal sealed class DocumentTypeUpdateHandler : IDocumentTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentTypeQueryService _documentTypeQueryService;
        public DocumentTypeUpdateHandler(
            IMapper mapper,
            IDocumentTypeQueryService documentTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentTypeQueryService = documentTypeQueryService;
            _documentTypeQueryService.NotNull(nameof(_documentTypeQueryService));
        }

        public async Task Handle(DocumentTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var documentType = await _documentTypeQueryService.Get(updateDto.Id);

            MemoryStream memoryStream = new MemoryStream();
            await (updateDto.Icon).CopyToAsync(memoryStream);
            string base64String = Convert.ToBase64String(memoryStream.ToArray());
            documentType.Icon = base64String;

            _mapper.Map(updateDto, documentType);
        }
    }
}
