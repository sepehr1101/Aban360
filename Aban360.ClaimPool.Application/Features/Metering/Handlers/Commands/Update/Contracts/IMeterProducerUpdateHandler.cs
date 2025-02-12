using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts
{
    public interface IMeterProducerUpdateHandler
    {
        Task Handle(MeterProducerUpdateDto updateDto, CancellationToken cancellationToken);
    }

}
