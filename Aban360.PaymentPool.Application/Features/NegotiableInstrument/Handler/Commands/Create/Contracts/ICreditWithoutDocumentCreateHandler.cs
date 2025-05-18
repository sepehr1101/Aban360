using Aban360.Common.ApplicationUser;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts
{
    public interface ICreditWithoutDocumentCreateHandler
    {
        Task Handle(IAppUser currentUser, CreditWithoutDocumentCreateDto createDto, Guid documentId, CancellationToken cancellationToken);
    }
}
