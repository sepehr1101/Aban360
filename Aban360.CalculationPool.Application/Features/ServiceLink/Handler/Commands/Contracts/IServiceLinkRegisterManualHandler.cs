using Aban360.CalculationPool.Domain.Features.ServiceLink;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts
{
    public interface IServiceLinkRegisterManualHandler
    {
        Task Handle(ServiceLinkRegisterManualInputDto input, CancellationToken cancellation);
    }
}
