using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Implementations
{
    internal sealed class OfficialHolidayGetSingleHandler : IOfficialHolidayGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfficialHolidayQueryService _officialHolidayQueryService;
        public OfficialHolidayGetSingleHandler(
            IMapper mapper,
            IOfficialHolidayQueryService officialHolidayQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _officialHolidayQueryService = officialHolidayQueryService;
            _officialHolidayQueryService.NotNull(nameof(_officialHolidayQueryService));
        }

        public async Task<OfficialHolidayGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var officialHoliday = await _officialHolidayQueryService.Get(id);
            return _mapper.Map<OfficialHolidayGetDto>(officialHoliday);
        }
    }
}
