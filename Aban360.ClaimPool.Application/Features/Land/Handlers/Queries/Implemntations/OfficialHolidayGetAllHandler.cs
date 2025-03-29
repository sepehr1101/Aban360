using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class OfficialHolidayGetAllHandler : IOfficialHolidayGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfficialHolidayQueryService _officialHolidayQueryService;
        public OfficialHolidayGetAllHandler(
            IMapper mapper,
            IOfficialHolidayQueryService officialHolidayQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _officialHolidayQueryService = officialHolidayQueryService;
            _officialHolidayQueryService.NotNull(nameof(_officialHolidayQueryService));
        }

        public async Task<ICollection<OfficialHolidayGetDto>> Handle(CancellationToken cancellationToken)
        {
            var officialHoliday = await _officialHolidayQueryService.Get();
            return _mapper.Map<ICollection<OfficialHolidayGetDto>>(officialHoliday);
        }
    }
}
