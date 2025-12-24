using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts
{
    public interface IHBedBesCommanddService
    {
        Task Insert(RemoveBillDataInputDto input);
    }
}
