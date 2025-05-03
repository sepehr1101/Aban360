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

        public async Task<Guid> Handle(DocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            Document document = new Document();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await createDto.DocumentFile.CopyToAsync(memoryStream);

                document.Id = Guid.NewGuid();
                document.FileRowId = Guid.NewGuid();
                document.Name = Path.GetFileNameWithoutExtension(createDto.DocumentFile.FileName);
                document.Extension = Path.GetExtension(createDto.DocumentFile.FileName).TrimStart('.');
                document.SizeInByte = memoryStream.Length;
                document.ContentType = createDto.DocumentFile.ContentType;
                document.FileContent = memoryStream.ToArray();
                document.CreatedDateTime = DateTime.Now;
                document.Description = createDto.Description;
                document.IsThumbnail = false;
                document.ParrentId = createDto.ParrentId;
                document.DocumentTypeId = createDto.DocumentTypeId;
            }

            await _documentCommandService.Add(document);
            return document.Id;
        }     
    }
}
