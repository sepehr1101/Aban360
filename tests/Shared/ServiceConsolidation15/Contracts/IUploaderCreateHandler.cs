// Test-only contract retained for pre-refactor snapshots.
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts
{
    public interface IUploaderCreateHandler
    {
        Task Handle(UploaderCreateDto createDto, CancellationToken cancellationToken);
    }
}
