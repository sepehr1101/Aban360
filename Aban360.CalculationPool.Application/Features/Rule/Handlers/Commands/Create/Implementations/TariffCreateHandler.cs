using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Implementations
{
    public class TariffCreateHandler : ITariffCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffCommandService _tariffCommandService;
        public TariffCreateHandler(
            IMapper mapper,
            ITariffCommandService tariffCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffCommandService = tariffCommandService;
            _tariffCommandService.NotNull(nameof(tariffCommandService));
        }

        public async Task Handle(TariffCreateDto createDto, CancellationToken cancellationToken)
        {
            var tariff = _mapper.Map<Tariff>(createDto);
            if (tariff == null)
            {
                throw new InvalidDataException();
            }
            await _tariffCommandService.Add(tariff);
        }
    }
}
