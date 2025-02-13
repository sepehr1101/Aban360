using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Implementations
{
    public class CountryCreateHandler : ICountryCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICountryCommandService _countryCommandService;
        public CountryCreateHandler(
            IMapper mapper,
            ICountryCommandService countryCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _countryCommandService = countryCommandService;
            _countryCommandService.NotNull(nameof(countryCommandService));
        }
        public async Task Handle(CountryCreateDto countryCreateDto, CancellationToken cancellationToken)
        {
            var coutry = _mapper.Map<Country>(countryCreateDto);
            await _countryCommandService.Add(coutry);
        }
    }
}
