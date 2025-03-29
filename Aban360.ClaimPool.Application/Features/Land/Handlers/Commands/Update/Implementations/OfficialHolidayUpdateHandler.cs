using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class OfficialHolidayUpdateHandler : IOfficialHolidayUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfficialHolidayQueryService _officialHolidayQueryService;
        public OfficialHolidayUpdateHandler(
            IMapper mapper,
            IOfficialHolidayQueryService officialHolidayQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _officialHolidayQueryService = officialHolidayQueryService;
            _officialHolidayQueryService.NotNull(nameof(_officialHolidayQueryService));
        }

        public async Task Handle(OfficialHolidayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var officialHoliday = await _officialHolidayQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, officialHoliday);
        }
    }
}
