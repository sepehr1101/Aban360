using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Implementations
{
    internal sealed class DocumentCategoryUpdateHandler : IDocumentCategoryUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentCategoryQueryService _documentCategoryQueryService;
        private readonly IValidator<DocumentCategoryUpdateDto> _validator;

        public DocumentCategoryUpdateHandler(
            IMapper mapper,
            IDocumentCategoryQueryService documentCategoryQueryService,
            IValidator<DocumentCategoryUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentCategoryQueryService = documentCategoryQueryService;
            _documentCategoryQueryService.NotNull(nameof(_documentCategoryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(DocumentCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var documentCategory = await _documentCategoryQueryService.Get(updateDto.Id);
            
            MemoryStream memoryStream = new MemoryStream();
            await (updateDto.Icon).CopyToAsync(memoryStream);
            string base64String = Convert.ToBase64String(memoryStream.ToArray());
            documentCategory.Icon = base64String;

            _mapper.Map(updateDto, documentCategory);
        }
    }
}
