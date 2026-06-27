using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts
{
    public interface IServiceLinkTempReturnHandler
    {
        Task Handle(ServiceLinkTempReturnInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
