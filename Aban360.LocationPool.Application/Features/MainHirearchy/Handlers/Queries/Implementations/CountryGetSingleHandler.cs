using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Implementations
{
    public class CountryGetSingleHandler : ICountryGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ICountryQueryService _countryQueryService;
        public CountryGetSingleHandler(
            IMapper mapper, 
            ICountryQueryService countryQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _countryQueryService = countryQueryService;
            _countryQueryService.NotNull(nameof(countryQueryService));
        }

        public async Task<CountryGetDto> Handle(short id,CancellationToken cancellationToken)
        {
            var country = await _countryQueryService.Get(id);
            return _mapper.Map<CountryGetDto>(country);
        }
    }
}
