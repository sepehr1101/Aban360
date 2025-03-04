using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Commands;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Implementations
{
    public class TariffCalculationModeDeleteHandler : ITariffCalculationModeDeleteHandler
    {
        private readonly ITariffCalculationModeCommandService _tariffCalculationModeCommandService;
        private readonly ITariffCalculationModeQueryService _tariffCalculationModeQueryService;
        public TariffCalculationModeDeleteHandler(
            ITariffCalculationModeCommandService tariffCalculationModeCommandService,
            ITariffCalculationModeQueryService tariffCalculationModeQueryService)
        {
            _tariffCalculationModeCommandService = tariffCalculationModeCommandService;
            _tariffCalculationModeCommandService.NotNull(nameof(tariffCalculationModeCommandService));

            _tariffCalculationModeQueryService = tariffCalculationModeQueryService;
            _tariffCalculationModeQueryService.NotNull(nameof(tariffCalculationModeQueryService));
        }

        public async Task Handle(TariffCalculationModeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var tariffCalculationMode = await _tariffCalculationModeQueryService.Get(deleteDto.Id);
            if (tariffCalculationMode == null)
            {
                throw new InvalidDataException();
            }
            await _tariffCalculationModeCommandService.Remove(tariffCalculationMode);
        }
    }
}
