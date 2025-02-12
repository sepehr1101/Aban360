using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts
{
    public interface IMeterProducerDeleteHandler
    {
        Task Handle(MeterProducerDeleteDto deleteDto, CancellationToken cancellationToken);
    }

}
