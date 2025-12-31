using Aban360.CalculationPool.Domain.Features.ServiceLink;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts
{
    public interface IServiceLinkInquiryDetailHandler
    {
        Task<IEnumerable<ServiceLinkInquiryOutputDto>> Handle(ServiceLinkInquiryInputDto input, CancellationToken cancellationToken);
    }
}
