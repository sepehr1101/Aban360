using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestFlatDeleteHandler : IRequestFlatDeleteHandler
    {
        private readonly IRequestFlatCommandService _requestFlatCommandService;
        private readonly IRequestFlatQueryService _requestFlatQueryService;
        public RequestFlatDeleteHandler(
            IRequestFlatCommandService requestFlatCommandService,
            IRequestFlatQueryService requestFlatQueryService)
        {
            _requestFlatCommandService = requestFlatCommandService;
            _requestFlatCommandService.NotNull(nameof(_requestFlatCommandService));

            _requestFlatQueryService = requestFlatQueryService;
            _requestFlatQueryService.NotNull(nameof(_requestFlatQueryService));
        }

        public async Task Handle(FlatRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestFlat = await _requestFlatQueryService.Get(deleteDto.Id);
            _requestFlatCommandService.Remove(requestFlat);
        }
    }
}
