using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;

namespace Aban360.ReportPool.Persistence.Features.Dashboard.Contracts
{
    public interface ITileScriptService
    {
        Task<int> Create(TileScript entity);
        Task<bool> Delete(int id, string deletedBy);
        Task<IEnumerable<TileScript>> GetAll();
        Task<TileScript?> GetById(int id);
        Task<bool> Update(TileScriptDto entity);
    }
}