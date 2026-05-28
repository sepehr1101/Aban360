using Aban360.CalculationPool.Domain.Features.ServiceLink;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts
{
    public interface IVosolEnQueryService
    {
        Task<IEnumerable<ServiceLinkPaidDataOutputDto>> Get(ServiceLinkPaidInputDto input);
        Task<ServiceLinkPaidDataOutputDto> Get(ServiceLinkPaymentRemoveInputDto input);
    }
}
