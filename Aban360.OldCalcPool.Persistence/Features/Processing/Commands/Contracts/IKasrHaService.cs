using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts
{
    public interface IKasrHaService
    {
        Task Create(ICollection<KasrHaDto> input);
    }
}
