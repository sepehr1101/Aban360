using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts
{
    public interface IServiceLinkReturnDisconnectHandler
    {
        Task<ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>> Handle(ServiceLinkReturnDisconnectInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
