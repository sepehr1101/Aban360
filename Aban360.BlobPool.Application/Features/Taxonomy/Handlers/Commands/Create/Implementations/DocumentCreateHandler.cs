using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Implementations
{
    internal sealed class DocumentCreateHandler : IDocumentCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentCommandService _documentCommandService;
        private readonly IValidator<DocumentCreateDto> _validator;
        public DocumentCreateHandler(
            IMapper mapper,
            IDocumentCommandService documentCommandService,
            IValidator<DocumentCreateDto> validator )
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentCommandService = documentCommandService;
            _documentCommandService.NotNull(nameof(_documentCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task<Guid> Handle(IFormFile documentFile,short documentTypeId,string? description, CancellationToken cancellationToken)
        {
            DocumentCreateDto documentCreateDto = new DocumentCreateDto()
            {
                DocumentFile = documentFile,
                Description = description,
                DocumentTypeId =documentTypeId,
            };
            var validationResult = await _validator.ValidateAsync(documentCreateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            Document document = new Document();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await documentCreateDto.DocumentFile.CopyToAsync(memoryStream);

                document.Id = Guid.NewGuid();
                document.FileRowId = Guid.NewGuid();
                document.Name = Path.GetFileNameWithoutExtension(documentCreateDto.DocumentFile.FileName);
                document.Extension = Path.GetExtension(documentCreateDto.DocumentFile.FileName).TrimStart('.');
                document.SizeInByte = memoryStream.Length;
                document.ContentType = documentCreateDto.DocumentFile.ContentType;
                document.FileContent = memoryStream.ToArray();
                document.CreatedDateTime = DateTime.Now;
                document.Description = documentCreateDto.Description;
                document.IsThumbnail = false;
                //document.ParrentId = documentCreateDto.ParrentId;
                document.DocumentTypeId = documentCreateDto.DocumentTypeId;
            }

            await _documentCommandService.Add(document);
            return document.Id;
        }     
    }
}
