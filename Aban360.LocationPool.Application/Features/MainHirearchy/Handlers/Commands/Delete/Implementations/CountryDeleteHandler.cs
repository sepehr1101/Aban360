using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Implementations;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Implementations
{
    public class CountryDeleteHandler : ICountryDeleteHandler
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
            var country = await _countryQueryService.Get(countryDeleteDto.Id);
            await _countryCommandService.Remove(country);
        }
    }
}
