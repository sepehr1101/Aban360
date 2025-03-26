using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Implementations
{
    internal sealed class OfficialHolidayCreateHandler : IOfficialHolidayCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfficialHolidayCommandService _officialHolidayCommandService;
        public OfficialHolidayCreateHandler(
            IMapper mapper,
            IOfficialHolidayCommandService officialHolidayCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _officialHolidayCommandService = officialHolidayCommandService;
            _officialHolidayCommandService.NotNull(nameof(_officialHolidayCommandService));
        }

        public async Task Handle(OfficialHolidayCreateDto createDto, CancellationToken cancellationToken)
        {
            var officialHoliday = _mapper.Map<OfficialHoliday>(createDto);
            await _officialHolidayCommandService.Add(officialHoliday);
        }
    }
}
