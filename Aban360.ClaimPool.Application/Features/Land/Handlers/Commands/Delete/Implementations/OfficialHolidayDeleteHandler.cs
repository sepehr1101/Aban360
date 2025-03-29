using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class OfficialHolidayDeleteHandler : IOfficialHolidayDeleteHandler
    {
        private readonly IOfficialHolidayCommandService _officialHolidayCommandService;
        private readonly IOfficialHolidayQueryService _officialHolidayQueryService;
        public OfficialHolidayDeleteHandler(
            IOfficialHolidayCommandService officialHolidayCommandService,
            IOfficialHolidayQueryService officialHolidayQueryService)
        {
            _officialHolidayCommandService = officialHolidayCommandService;
            _officialHolidayCommandService.NotNull(nameof(_officialHolidayCommandService));

            _officialHolidayQueryService = officialHolidayQueryService;
            _officialHolidayQueryService.NotNull(nameof(_officialHolidayQueryService));
        }

        public async Task Handle(OfficialHolidayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var officialHoliday = await _officialHolidayQueryService.Get(deleteDto.Id);
            await _officialHolidayCommandService.Remove(officialHoliday);
        }
    }
}
