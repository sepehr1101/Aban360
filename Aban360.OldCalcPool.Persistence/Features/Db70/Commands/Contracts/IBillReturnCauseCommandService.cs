using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts
{
    public interface IBillReturnCauseCommandService
    {
        Task Create(BillReturnCauseCreateDto input);
        Task Update(BillReturnCauseUpdateDto input);
        Task Delete(BillReturnCauseDeleteDto input);
    }
}
