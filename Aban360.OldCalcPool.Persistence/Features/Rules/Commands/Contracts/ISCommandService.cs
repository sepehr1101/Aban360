using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts
{
    public interface ISCommandService
    {
        Task Update(SUpdateDto input);
        Task Create(SCreateDto input);
        Task Delete(int id);
    }
}
