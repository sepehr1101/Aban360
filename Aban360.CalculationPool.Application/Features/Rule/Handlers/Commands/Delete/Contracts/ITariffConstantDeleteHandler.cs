using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts
{
    public interface ITariffConstantDeleteHandler
    {
        Task Handle(TariffConstantDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
