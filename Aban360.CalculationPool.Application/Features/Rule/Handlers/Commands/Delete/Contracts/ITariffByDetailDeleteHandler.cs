using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts
{
    public interface ITariffByDetailDeleteHandler
    {
        Task Handle(TariffByDetailDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
