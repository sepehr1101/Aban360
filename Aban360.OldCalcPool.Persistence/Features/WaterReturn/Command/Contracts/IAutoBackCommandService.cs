using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Contracts
{
    public interface IAutoBackCommandService
    {
        Task Create(AutoBackCreateDto input);
    }
}