using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Implementations
{
    public class TariffConstantDeleteHandler : ITariffConstantDeleteHandler
    {
        private readonly ITariffConstantCommandService _tariffConstantCommandService;
        private readonly ITariffConstantQueryService _tariffConstantQueryService;
        public TariffConstantDeleteHandler(
            ITariffConstantCommandService tariffConstantCommandService,
            ITariffConstantQueryService tariffConstantQueryService)
        {
            _tariffConstantCommandService = tariffConstantCommandService;
            _tariffConstantCommandService.NotNull(nameof(tariffConstantCommandService));

            _tariffConstantQueryService = tariffConstantQueryService;
            _tariffConstantQueryService.NotNull(nameof(tariffConstantQueryService));
        }

        public async Task Handle(TariffConstantDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var tariffConstant = await _tariffConstantQueryService.Get(deleteDto.Id);
            if (tariffConstant == null)
            {
                throw new InvalidDataException();
            }
            await _tariffConstantCommandService.Remove(tariffConstant);
        }
    }
}
