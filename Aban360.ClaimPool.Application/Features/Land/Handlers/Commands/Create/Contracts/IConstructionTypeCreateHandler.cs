using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface IConstructionTypeCreateHandler
    {
        Task Handle(ConstructionTypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
