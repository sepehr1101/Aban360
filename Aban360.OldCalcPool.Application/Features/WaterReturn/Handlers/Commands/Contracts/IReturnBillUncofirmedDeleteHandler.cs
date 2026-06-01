using Aban360.Common.ApplicationUser;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IReturnBillUncofirmedDeleteHandler
    {
        Task Handle(int confirmedNumber, IAppUser appUser, CancellationToken cancellationToken);
    }
}
