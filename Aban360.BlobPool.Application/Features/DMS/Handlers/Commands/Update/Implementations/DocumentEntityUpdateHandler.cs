using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Persistence.Features.DMS.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;
using Aban360.Common.Exceptions;

namespace Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Update.Implementations
{
    internal sealed class DocumentEntityUpdateHandler : IDocumentEntityUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentEntityQueryService _documentEntityQueryService;
        private readonly IValidator<DocumentEntityUpdateDto> _validator;
        public DocumentEntityUpdateHandler(
            IMapper mapper,
            IDocumentEntityQueryService documentEntityQueryService,
            IValidator<DocumentEntityUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentEntityQueryService = documentEntityQueryService;
            _documentEntityQueryService.NotNull(nameof(_documentEntityQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(DocumentEntityUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var documentEntity = await _documentEntityQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, documentEntity);
        }
    }
}
