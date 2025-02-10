using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts
{
    public interface IEndpointQueryService
    {
        Task<ICollection<Endpoint>> Get();
        Task<ICollection<Endpoint>> GetInclude();
        Task<Endpoint> Get(int id);
        Task<Endpoint> GetInclude(int id);
        IQueryable<Endpoint> GetQuery();
    }
}