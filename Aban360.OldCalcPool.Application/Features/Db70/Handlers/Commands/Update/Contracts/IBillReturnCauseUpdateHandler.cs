using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Update.Contracts
{
    public interface IBillReturnCauseUpdateHandler
    {
        Task Handle(BillReturnCauseUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
