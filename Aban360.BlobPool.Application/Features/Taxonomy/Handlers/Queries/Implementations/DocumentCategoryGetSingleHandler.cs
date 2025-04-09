using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Implementations
{
    internal sealed class DocumentCategoryGetSingleHandler : IDocumentCategoryGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentCategoryQueryService _documentCategoryQueryService;
        public DocumentCategoryGetSingleHandler(
            IMapper mapper,
            IDocumentCategoryQueryService documentCategoryQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentCategoryQueryService = documentCategoryQueryService;
            _documentCategoryQueryService.NotNull(nameof(_documentCategoryQueryService));
        }

        public async Task<DocumentCategoryGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var documentCategory = await _documentCategoryQueryService.Get(id);
            return _mapper.Map<DocumentCategoryGetDto>(documentCategory);
        }
    }
}
