using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts
{
    public interface IServiceLinkUnconfirmedGetHandler
    {
        Task<ReportOutput<ServiceLinkUnconfirmedHeaderOutputDto, ServiceLinkUnconfirmedDataOutputDto>> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
