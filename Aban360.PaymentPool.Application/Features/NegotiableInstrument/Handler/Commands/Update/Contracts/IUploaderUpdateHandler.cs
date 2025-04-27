using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Contracts
{
    public interface IUploaderUpdateHandler
    {
        Task Handle(UploaderUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
