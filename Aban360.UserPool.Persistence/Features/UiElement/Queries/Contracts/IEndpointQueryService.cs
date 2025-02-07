using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts
{
    public interface IEndpointQueryService
    {
        Task<ICollection<Endpoint>> Get();
        Task<Endpoint> Get(int id);
        IQueryable<Endpoint> GetQuery();
    }
}