using Aban360.CalculationPool.Domain.Features.ServiceLink;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts
{
    public interface IServiceLinkDetailQueryService
    {
        Task<IEnumerable<ServiceLinkInquiryOutputDto>> Get(ServiceLinkInquiryInputDto input);
    }
}
