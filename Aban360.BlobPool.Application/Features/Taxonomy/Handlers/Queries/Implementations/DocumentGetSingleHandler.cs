using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Implementations
{
    internal sealed class DocumentGetSingleHandler : IDocumentGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentQueryService _documentQueryService;
        public DocumentGetSingleHandler(
            IMapper mapper,
            IDocumentQueryService documentQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentQueryService = documentQueryService;
            _documentQueryService.NotNull(nameof(_documentQueryService));
        }

        public async Task<DocumentGetDto> Handle(Guid id, CancellationToken cancellationToken)
        {
            var document = await _documentQueryService.Get(id);
            return _mapper.Map<DocumentGetDto>(document);
        }
    }
}
