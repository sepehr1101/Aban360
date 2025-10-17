using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Implementations
{
    internal sealed class DocumentCategoryCreateHandler : IDocumentCategoryCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentCategoryCommandService _documentCategoryCommandService;
        private readonly IValidator<DocumentCategoryCreateDto> _validator;
        public DocumentCategoryCreateHandler(
            IMapper mapper,
            IDocumentCategoryCommandService documentCategoryCommandService,
            IValidator<DocumentCategoryCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentCategoryCommandService = documentCategoryCommandService;
            _documentCategoryCommandService.NotNull(nameof(_documentCategoryCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(DocumentCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }


            var documentCategory = _mapper.Map<DocumentCategory>(createDto);

            MemoryStream memoryStream = new MemoryStream();
            await (createDto.Icon).CopyToAsync(memoryStream);
            string base64String = Convert.ToBase64String(memoryStream.ToArray());
            documentCategory.Icon = base64String;

            await _documentCategoryCommandService.Add(documentCategory);
        }
    }
}
