using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
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

            _officialHolidayCommandService = officialHolidayCommandService ?? throw new ArgumentNullException(nameof(officialHolidayCommandService));
            _officialHolidayCommandService.NotNull(nameof(_officialHolidayCommandService));
        }

        public async Task Handle(OfficialHolidayCreateDto createDto, CancellationToken cancellationToken)
        {
            var officialHoliday = _mapper.Map<OfficialHoliday>(createDto);
            await _officialHolidayCommandService.Add(officialHoliday);
        }
    }
}
