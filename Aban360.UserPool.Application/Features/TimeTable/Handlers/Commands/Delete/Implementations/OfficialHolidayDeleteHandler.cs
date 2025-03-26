using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Implementations
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
