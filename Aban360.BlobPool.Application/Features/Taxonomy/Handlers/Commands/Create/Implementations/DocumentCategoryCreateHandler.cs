using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Implementations
{
    internal sealed class DocumentCategoryCreateHandler : IDocumentCategoryCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentCategoryCommandService _documentCategoryCommandService;
        public DocumentCategoryCreateHandler(
            IMapper mapper,
            IDocumentCategoryCommandService documentCategoryCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentCategoryCommandService = documentCategoryCommandService;
            _documentCategoryCommandService.NotNull(nameof(_documentCategoryCommandService));
        }

        public async Task Handle(DocumentCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            var documentCategory = _mapper.Map<DocumentCategory>(createDto);

            MemoryStream memoryStream = new MemoryStream();
            await (createDto.Icon).CopyToAsync(memoryStream);
            string base64String = Convert.ToBase64String(memoryStream.ToArray());
            documentCategory.Icon = base64String;

            await _documentCategoryCommandService.Add(documentCategory);
        }
    }
}
