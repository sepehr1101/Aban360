using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Contracts
{
    public interface IBillReturnCauseDeleteHandler
    {
        Task Handle(BillReturnCauseDeleteDto deleteDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
