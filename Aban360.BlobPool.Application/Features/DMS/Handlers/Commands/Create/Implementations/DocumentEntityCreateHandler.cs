using Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.BlobPool.Domain.Features.DMS.Entities;
using Aban360.BlobPool.Persistence.Features.DMS.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Create.Implementations
{
    internal sealed class DocumentEntityCreateHandler : IDocumentEntityCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentEntityCommandService _documentEntityCommandService;
        private readonly IValidator<DocumentEntityCreateDto > _documentEntityCreateValidator;
        public DocumentEntityCreateHandler(
            IMapper mapper,
            IDocumentEntityCommandService documentEntityCommandService,
            IValidator<DocumentEntityCreateDto> documentEntityCreateValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentEntityCommandService = documentEntityCommandService;
            _documentEntityCommandService.NotNull(nameof(_documentEntityCommandService));

            _documentEntityCreateValidator= documentEntityCreateValidator;
            _documentEntityCreateValidator.NotNull(nameof(_documentEntityCreateValidator)); 
        }

        public async Task Handle(DocumentEntityCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _documentEntityCreateValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var documentEntity = _mapper.Map<DocumentEntity>(createDto);
            await _documentEntityCommandService.Add(documentEntity);
        }
    }
}
