using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface ICapacityCalculationIndexCreateHandler
    {
        Task Handle(CapacityCalculationIndexCreateDto createDto, CancellationToken cancellationToken);
    }
}
