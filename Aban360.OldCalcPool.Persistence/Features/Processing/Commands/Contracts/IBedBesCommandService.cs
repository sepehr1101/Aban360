using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts
{
    public interface IBedBesCommandService
    {
        Task Create(BedBesCreateDto input, int zoneId);
        Task Create(ICollection<BedBesCreateDto> input);
        Task Create(ICollection<BedBesCreateDto> input, int zoneId);
        Task Delete(int id);
        Task UpdateDel(IEnumerable<BedBesUpdateDelDto> input);
        Task UpdateDel(BedBesUpdateDelWithDateDto input);
    }
}
