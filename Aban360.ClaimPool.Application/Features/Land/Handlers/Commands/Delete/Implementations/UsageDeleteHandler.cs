using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class UsageDeleteHandler : IUsageDeleteHandler
    {
        private readonly IUsageQuerySevice _usageQueryService;
        private readonly IUsageCommandSevice _usageCommandService;
        public UsageDeleteHandler(
           IUsageQuerySevice usageQueryService,
            IUsageCommandSevice usageCommandService)
        {
            _usageQueryService = usageQueryService;
            _usageQueryService.NotNull(nameof(usageQueryService));

            _usageCommandService = usageCommandService;
            _usageCommandService.NotNull(nameof(usageCommandService));
        }

        public async Task Handle(UsageDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Usage usage = await _usageQueryService.Get(deleteDto.Id);
            if (usage == null)
            {
                throw new InvalidDataException();
            }
            await _usageCommandService.Remove(usage);
        }
    }
}
