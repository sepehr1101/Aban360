using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Implementations
{
    internal sealed class TariffDeleteHandler : ITariffDeleteHandler
    {
        private readonly ITariffCommandService _tariffCommandService;
        private readonly ITariffQueryService _tariffQueryService;
        public TariffDeleteHandler(
            ITariffCommandService tariffCommandService,
            ITariffQueryService tariffQueryService)
        {
            _tariffCommandService = tariffCommandService;
            _tariffCommandService.NotNull(nameof(tariffCommandService));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(tariffQueryService));
        }

        public async Task Handle(TariffDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Tariff tariff = await _tariffQueryService.Get(deleteDto.Id);
            await _tariffCommandService.Remove(tariff);
        }
    }
}
