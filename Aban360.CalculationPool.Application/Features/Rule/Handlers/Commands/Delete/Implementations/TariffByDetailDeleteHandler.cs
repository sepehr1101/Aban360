using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Implementations
{
    internal sealed class TariffByDetailDeleteHandler : ITariffByDetailDeleteHandler
    {
        private readonly ITariffByDetailCommandService _tariffByDetailCommandService;
        private readonly ITariffByDetailQueryService _tariffByDetailQueryService;
        public TariffByDetailDeleteHandler(
            ITariffByDetailCommandService tariffByDetailCommandService,
            ITariffByDetailQueryService tariffByDetailQueryService)
        {
            _tariffByDetailCommandService = tariffByDetailCommandService;
            _tariffByDetailCommandService.NotNull(nameof(_tariffByDetailCommandService));

            _tariffByDetailQueryService = tariffByDetailQueryService;
            _tariffByDetailQueryService.NotNull(nameof(_tariffByDetailQueryService));
        }

        public async Task Handle(TariffByDetailDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var tariffByDetail = await _tariffByDetailQueryService.Get(deleteDto.Id);
            await _tariffByDetailCommandService.Remove(tariffByDetail);
        }
    }
}
