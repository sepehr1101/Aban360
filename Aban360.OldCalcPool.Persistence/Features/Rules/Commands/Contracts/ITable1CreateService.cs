using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts
{
    public interface ITable1CreateService
    {
        Task Create(Table1CreateDto input);
    }
}
