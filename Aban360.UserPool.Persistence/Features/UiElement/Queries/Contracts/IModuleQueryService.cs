using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts
{
    public interface IModuleQueryService
    {
        Task<ICollection<Module>> Get();
        Task<ICollection<Module>> GetInclude();
        Task<Module> Get(int id);
        Task<Module> GetInclude(int id);
        IQueryable<Module> GetQuery();
    }
}