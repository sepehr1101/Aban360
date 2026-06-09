using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts
{
    public interface IServiceLinkReturnHandler
    {
        Task Handle(ServiceLinkReturnInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        IEnumerable<NumericDictionary> ReturnCodes();
    }
}
