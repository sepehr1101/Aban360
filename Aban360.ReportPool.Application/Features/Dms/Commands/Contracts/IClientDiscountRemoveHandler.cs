using Aban360.Common.ApplicationUser;

namespace Aban360.ReportPool.Application.Features.Dms.Commands.Contracts
{
    public interface IClientDiscountRemoveHandler
    {
        Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken);
    }
}
