using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts
{
    public interface ISubModuleQueryService
    {
        Task<ICollection<SubModule>> Get();
        Task<SubModule> Get(int id);
        IQueryable<SubModule> GetQuery();
    }
}