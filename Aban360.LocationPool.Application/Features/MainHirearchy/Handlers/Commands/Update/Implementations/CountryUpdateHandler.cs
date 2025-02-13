using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Implementations
{
    public class CountryUpdateHandler : ICountryUpdateHandler
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
            var country = await _countryQueryService.Get(countryUpdateDto.Id);
            if (country == null)
            {
                throw new InvalidDataException();//todo: create ExceptionClass
            }

            _mapper.Map(countryUpdateDto, country);
        }
    }
}
