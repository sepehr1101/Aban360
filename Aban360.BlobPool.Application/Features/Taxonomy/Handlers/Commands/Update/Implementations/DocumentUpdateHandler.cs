using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Implementations
{
    internal sealed class DocumentUpdateHandler : IDocumentUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentQueryService _documentQueryService;
        public DocumentUpdateHandler(
            IMapper mapper,
            IDocumentQueryService documentQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentQueryService = documentQueryService;
            _documentQueryService.NotNull(nameof(_documentQueryService));
        }

        public async Task Handle(DocumentUpdateDto updateDto, CancellationToken cancellationToken)
        {
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
