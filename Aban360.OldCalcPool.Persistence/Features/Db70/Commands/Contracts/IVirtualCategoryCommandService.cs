using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts
{
    public interface IVirtualCategoryCommandService
    {
        Task Create(VirtualCategoryCreateDto input);
        Task Update(VirtualCategoryUpdateDto input);
        Task Delete(VirtualCategorySearchInputDto input);
    }
}
