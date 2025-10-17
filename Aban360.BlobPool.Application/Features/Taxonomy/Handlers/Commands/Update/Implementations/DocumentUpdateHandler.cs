using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Implementations
{
    internal sealed class DocumentUpdateHandler : IDocumentUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentQueryService _documentQueryService;
        private readonly IValidator<DocumentUpdateDto> _validator;

        public DocumentUpdateHandler(
            IMapper mapper,
            IDocumentQueryService documentQueryService,
            IValidator<DocumentUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentQueryService = documentQueryService;
            _documentQueryService.NotNull(nameof(_documentQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(DocumentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var document = await _documentQueryService.Get(updateDto.Id);

            MemoryStream memoryStream = new MemoryStream();
            await updateDto.document.CopyToAsync(memoryStream);

            document.FileRowId = Guid.NewGuid();//ToDo
            document.Name = Path.GetFileNameWithoutExtension(updateDto.document.FileName);
            document.Extension = Path.GetExtension(updateDto.document.FileName).TrimStart('.');
            document.SizeInByte = memoryStream.Length;
            document.ContentType = updateDto.document.ContentType;
            document.FileContent = memoryStream.ToArray();
            document.CreatedDateTime = DateTime.Now;
            document.Description = updateDto.Description;
            document.IsThumbnail = updateDto.IsThumbnail;
            document.ParrentId = updateDto.ParrentId;

            _mapper.Map(updateDto, document);
        }
    }
}
