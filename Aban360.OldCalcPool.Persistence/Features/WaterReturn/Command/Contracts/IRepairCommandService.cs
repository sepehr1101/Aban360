using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts
{
    public interface IRepairCommandService
    {
        Task Create(RepairCreateDto input);
        Task Create(IEnumerable<RepairCreateDto> input);
        Task Delete(RepairDeleteDto input);
        Task Update(RepairUpdateDto input);
    }
}
