using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts
{
    public interface ISubModuleQueryService
    {
        Task<ICollection<SubModule>> Get();
        Task<ICollection<SubModule>> GetInclude();
        Task<SubModule> Get(int id);
        Task<SubModule> GetInclude(int id);
        IQueryable<SubModule> GetQuery();
    }
}