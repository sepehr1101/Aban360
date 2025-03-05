using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts
{
    public interface ITariffUpdateHandler
    {
        Task Handle(TariffUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
