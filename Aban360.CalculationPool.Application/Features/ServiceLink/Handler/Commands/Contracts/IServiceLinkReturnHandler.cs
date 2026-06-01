using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts
{
    public interface IServiceLinkReturnHandler
    {
        Task Handle(ServiceLinkReturnInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
