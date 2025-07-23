using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts
{
    public interface IZaribUpdateService
    {
        Task Update(ZaribUpdateDto input);
    }
}
