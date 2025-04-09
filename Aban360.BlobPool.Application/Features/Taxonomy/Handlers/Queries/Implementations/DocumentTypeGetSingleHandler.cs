using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Implementations
{
    internal sealed class DocumentTypeGetSingleHandler : IDocumentTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentTypeQueryService _documentTypeQueryService;
        public DocumentTypeGetSingleHandler(
            IMapper mapper,
            IDocumentTypeQueryService documentTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentTypeQueryService = documentTypeQueryService;
            _documentTypeQueryService.NotNull(nameof(_documentTypeQueryService));
        }

        public async Task<DocumentTypeGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var documentType = await _documentTypeQueryService.Get(id);
            return _mapper.Map<DocumentTypeGetDto>(documentType);
        }
    }
}
