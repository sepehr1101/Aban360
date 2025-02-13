using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IConstructionTypeCommandService
    {
        Task Add(ConstructionType constructionType);
        Task Remove(ConstructionType constructionType);
    }
}
