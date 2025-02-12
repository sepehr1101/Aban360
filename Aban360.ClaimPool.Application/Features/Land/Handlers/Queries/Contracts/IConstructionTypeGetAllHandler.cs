using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface IConstructionTypeGetAllHandler
    {
        Task<ICollection<ConstructionTypeGetDto>> Handle(CancellationToken cancellationToken);
    }

}
