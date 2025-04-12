using Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.DMS.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Update.Implementations
{
    internal sealed class DocumentEntityUpdateHandler : IDocumentEntityUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentEntityQueryService _documentEntityQueryService;
        public DocumentEntityUpdateHandler(
            IMapper mapper,
            IDocumentEntityQueryService documentEntityQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentEntityQueryService = documentEntityQueryService;
            _documentEntityQueryService.NotNull(nameof(_documentEntityQueryService));
        }

        public async Task Handle(DocumentEntityUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var documentEntity = await _documentEntityQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, documentEntity);
        }
    }
}
