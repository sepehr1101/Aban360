using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts
{
    public interface ITariffConstantCreateHandler
    {
        Task Handle(TariffConstantCreateDto createDto, CancellationToken cancellationToken);
    }
}
