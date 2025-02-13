using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Implementations
{
    public class CordinalDirectionDeleteHandler : ICordinalDirectionDeleteHandler
    {
        private readonly ICordinalDirectionCommandService _cordinalDirectionCommandService;
        private readonly ICordinalDirectionQueryService _cordinalDirectionQueryService;
        public CordinalDirectionDeleteHandler(
            ICordinalDirectionQueryService cordinalDirectionQueryService,
            ICordinalDirectionCommandService cordinalDirectionCommandService)
        {
            _cordinalDirectionCommandService = cordinalDirectionCommandService;
            _cordinalDirectionCommandService.NotNull(nameof(cordinalDirectionCommandService));

            _cordinalDirectionQueryService = cordinalDirectionQueryService;
            _cordinalDirectionQueryService.NotNull(nameof(cordinalDirectionQueryService));
        }

        public async Task Handle(CordinalDirectionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var cordinalDirection =await _cordinalDirectionQueryService.Get(deleteDto.Id);
            if (cordinalDirection == null)
            {
                throw new InvalidDataException();
            }
            await _cordinalDirectionCommandService.Remove(cordinalDirection);
        }
    }
}
