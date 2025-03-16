using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Implementations
{
    internal sealed class UseStateDeleteHandler : IUseStateDeleteHandler
    {
        private readonly IUseStateCommandService _useStateCommandService;
        private readonly IUseStateQueryService _useStateQueryService;
        public UseStateDeleteHandler(
            IUseStateCommandService useStateCommandService,
            IUseStateQueryService useStateQueryService)
        {
            _useStateCommandService = useStateCommandService;
            _useStateCommandService.NotNull(nameof(useStateCommandService));

            _useStateQueryService = useStateQueryService;
            _useStateQueryService.NotNull(nameof(useStateQueryService));
        }

        public async Task Handle(UseStateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            UseState useState = await _useStateQueryService.Get(deleteDto.Id);
            if (useState == null)
            {
                throw new InvalidDataException();
            }
            await _useStateCommandService.Remove(useState);
        }
    }
}
