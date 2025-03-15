using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Implementations
{
    internal sealed class CountryDeleteHandler : ICountryDeleteHandler
    {
        private readonly ICountryCommandService _countryCommandService;
        private readonly ICountryQueryService _countryQueryService;
        public CountryDeleteHandler(
            ICountryCommandService countryCommandService,
            ICountryQueryService countryQueryService)
        {
            _countryCommandService = countryCommandService;
            _countryCommandService.NotNull(nameof(countryCommandService));

            _countryQueryService = countryQueryService;
            _countryQueryService.NotNull(nameof(countryQueryService));
        }
        public async Task Handle(CountryDeleteDto countryDeleteDto, CancellationToken cancellationToken)
        {
            Country country = await _countryQueryService.Get(countryDeleteDto.Id);
            await _countryCommandService.Remove(country);
        }
    }
}
