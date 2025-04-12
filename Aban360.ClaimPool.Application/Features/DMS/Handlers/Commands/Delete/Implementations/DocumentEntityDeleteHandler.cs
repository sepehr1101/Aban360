using Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.DMS.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.DMS.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Delete.Implementations
{
    internal sealed class DocumentEntityDeleteHandler : IDocumentEntityDeleteHandler
    {
        private readonly IDocumentEntityCommandService _documentEntityCommandService;
        private readonly IDocumentEntityQueryService _documentEntityQueryService;
        public DocumentEntityDeleteHandler(
            IDocumentEntityCommandService documentEntityCommandService,
            IDocumentEntityQueryService documentEntityQueryService)
        {
            _documentEntityCommandService = documentEntityCommandService;
            _documentEntityCommandService.NotNull(nameof(_documentEntityCommandService));

            _documentEntityQueryService = documentEntityQueryService;
            _documentEntityQueryService.NotNull(nameof(_documentEntityQueryService));
        }

        public async Task Handle(DocumentEntityDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var documentEntity = await _documentEntityQueryService.Get(deleteDto.Id);
            await _documentEntityCommandService.Remove(documentEntity);
        }
    }
}
