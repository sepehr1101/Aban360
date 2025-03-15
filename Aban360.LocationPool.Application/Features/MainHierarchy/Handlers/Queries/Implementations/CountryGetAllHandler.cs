using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class CountryGetAllHandler : ICountryGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ICountryQueryService _countryQueryService;
        public CountryGetAllHandler(
            IMapper mapper, 
            ICountryQueryService countryQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _countryQueryService = countryQueryService;
            _countryQueryService.NotNull(nameof(countryQueryService));
        }

        public async Task<ICollection<CountryGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Country> countries = await _countryQueryService.Get();
            return _mapper.Map<ICollection<CountryGetDto>>(countries);  
        }
    }
}
