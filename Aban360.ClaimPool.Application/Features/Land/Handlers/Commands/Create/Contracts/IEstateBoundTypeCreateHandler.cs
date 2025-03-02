using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface IEstateBoundTypeCreateHandler
    {
        Task Handle(EstateBoundTypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
