using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Implementations
{
    internal sealed class DocumentGetAllHandler : IDocumentGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentQueryService _documentQueryService;
        public DocumentGetAllHandler(
            IMapper mapper,
            IDocumentQueryService documentQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentQueryService = documentQueryService;
            _documentQueryService.NotNull(nameof(_documentQueryService));
        }

        public async Task<ICollection<DocumentGetDto>> Handle(CancellationToken cancellationToken)
        {
            var document = await _documentQueryService.Get();
            return _mapper.Map<ICollection<DocumentGetDto>>(document);
        }
    }
}
