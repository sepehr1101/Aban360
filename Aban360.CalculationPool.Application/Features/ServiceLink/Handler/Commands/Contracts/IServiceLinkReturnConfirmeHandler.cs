using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts
{
    public interface IServiceLinkReturnConfirmeHandler
    {
        Task Handle(string billId, IAppUser appUser, CancellationToken cancellationToken);
    }
}