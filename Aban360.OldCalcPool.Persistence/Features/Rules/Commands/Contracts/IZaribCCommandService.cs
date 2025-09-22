using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts
{
    public interface IZaribCCommandService
    {
        Task Update(ZaribCUpdateDto input);
        Task Create(ZaribCCreateDto input);
        Task Delete (int id);
    }
}
