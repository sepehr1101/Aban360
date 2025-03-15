using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    internal sealed class CountryUpdateHandler : ICountryUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICountryQueryService _countryQueryService;
        public CountryUpdateHandler(
            IMapper mapper,
            ICountryQueryService countryQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _countryQueryService = countryQueryService;
            _countryQueryService.NotNull(nameof(countryQueryService));
        }
        public async Task Handle(CountryUpdateDto countryUpdateDto, CancellationToken cancellationToken)
        {
            Country country = await _countryQueryService.Get(countryUpdateDto.Id);
            _mapper.Map(countryUpdateDto, country);
        }
    }
}
