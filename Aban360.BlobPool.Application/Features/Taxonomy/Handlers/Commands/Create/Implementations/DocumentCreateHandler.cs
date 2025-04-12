using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Implementations
{
    internal sealed class DocumentCreateHandler : IDocumentCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentCommandService _documentCommandService;
        public DocumentCreateHandler(
            IMapper mapper,
            IDocumentCommandService documentCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentCommandService = documentCommandService;
            _documentCommandService.NotNull(nameof(_documentCommandService));
        }

        public async Task<Guid> Handle(DocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            Document document = new Document();


            MemoryStream memoryStream = new MemoryStream();
            await createDto.document.CopyToAsync(memoryStream);

            document.Id = Guid.NewGuid();
            document.FileRowId = Guid.NewGuid();//ToDo
            document.Name = Path.GetFileNameWithoutExtension(createDto.document.FileName);
            document.Extension = Path.GetExtension(createDto.document.FileName).TrimStart('.');
            document.SizeInByte = memoryStream.Length;
            document.ContentType= createDto.document.ContentType;
            document.FileContent = memoryStream.ToArray();
            document.CreatedDateTime = DateTime.Now;
            document.Description = createDto.Description;
            document.IsThumbnail = createDto.IsThumbnail;
            document.ParrentId=createDto.ParrentId;
            document.DocumentTypeId = createDto.DocumentTypeId;

            await _documentCommandService.Add(document);
            return document.Id;
        }
    }
}
