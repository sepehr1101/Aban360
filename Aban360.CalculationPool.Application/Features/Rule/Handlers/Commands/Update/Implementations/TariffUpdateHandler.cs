using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Implementations
{
    public class TariffUpdateHandler : ITariffUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffQueryService _tariffQueryService;
        public TariffUpdateHandler(
            IMapper mapper,
            ITariffQueryService tariffQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(tariffQueryService));
        }

        public async Task Handle(TariffUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var tariff = await _tariffQueryService.Get(updateDto.Id);
            if (tariff == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, tariff);
        }
    }
}
