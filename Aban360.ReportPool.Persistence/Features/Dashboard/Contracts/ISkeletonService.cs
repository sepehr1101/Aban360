using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;

namespace Aban360.ReportPool.Persistence.Features.Dashboard.Contracts
{
    public interface ISkeletonService
    {
        Task<int> Create(Skeleton entity);
        Task<bool> Delete(int id, string deletedBy);
        Task<Skeleton?> GetById(int id);
        Task<Skeleton?> GetByRole(string role);
        Task<IEnumerable<Skeleton>?> GetAllByRole(string role);
        Task<IEnumerable<SkeletonDefinitionDto>> GetDefinitions();
        Task<bool> Update(Skeleton entity);
    }
}