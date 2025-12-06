using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts
{
    public interface IKasrHaService
    {
        Task Create(KasrHaDto input, int zoneId);
        Task Create(ICollection<KasrHaDto> input);
        Task Create(ICollection<KasrHaDto> input, int zoneId);
        Task Delete(RemovedBillInputDto input);
    }
}
