using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Commands;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Implementations
{
    public class TariffCalculationModeUpdateHandler : ITariffCalculationModeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffCalculationModeQueryService _tariffCalculationModeQueryService;
        public TariffCalculationModeUpdateHandler(
            IMapper mapper,
            ITariffCalculationModeQueryService tariffCalculationModeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffCalculationModeQueryService = tariffCalculationModeQueryService;
            _tariffCalculationModeQueryService.NotNull(nameof(tariffCalculationModeQueryService));
        }

        public async Task Handle(TariffCalculationModeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var tariffCalculationMode = await _tariffCalculationModeQueryService.Get(updateDto.Id);
            if (tariffCalculationMode == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, tariffCalculationMode);
        }
    }
}
