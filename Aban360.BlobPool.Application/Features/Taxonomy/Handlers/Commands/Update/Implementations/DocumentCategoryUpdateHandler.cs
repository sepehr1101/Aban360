using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Implementations
{
    internal sealed class DocumentCategoryUpdateHandler : IDocumentCategoryUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentCategoryQueryService _documentCategoryQueryService;
        public DocumentCategoryUpdateHandler(
            IMapper mapper,
            IDocumentCategoryQueryService documentCategoryQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentCategoryQueryService = documentCategoryQueryService;
            _documentCategoryQueryService.NotNull(nameof(_documentCategoryQueryService));
        }

        public async Task Handle(DocumentCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var documentCategory = await _documentCategoryQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, documentCategory);
        }
    }
}
