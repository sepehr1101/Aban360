using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Implementations
{
    public class TariffCalculationModeCreateHandler : ITariffCalculationModeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffCalculationModeCommandService _tariffCalculationModeCommandService;
        public TariffCalculationModeCreateHandler(
            IMapper mapper,
            ITariffCalculationModeCommandService tariffCalculationModeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffCalculationModeCommandService = tariffCalculationModeCommandService;
            _tariffCalculationModeCommandService.NotNull(nameof(tariffCalculationModeCommandService));
        }

        public async Task Handle(TariffCalculationModeCreateDto createDto, CancellationToken cancellationToken)
        {
            var tariffCalculationMode = _mapper.Map<TariffCalculationMode>(createDto);
            if (tariffCalculationMode == null)
            {
                throw new InvalidDataException();
            }
            await _tariffCalculationModeCommandService.Add(tariffCalculationMode);
        }
    }
}
