using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts
{
    public interface IAppQueryService
    {
        Task<ICollection<App>> Get();
        Task<App> Get(int id);
        IQueryable<App> GetQuery();
    }
}