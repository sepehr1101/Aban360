using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    public interface IOtherExpensesItemsRemoveHandler
    {
        Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken);
    }
}
