using Aban360.Common.ApplicationUser;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts
{
    public interface ICreditByDocumentCreateHandler
    {
        Task Handle(IAppUser currentUser,CreditByDocumentCreateDto createDto, CancellationToken cancellationToken);
    }
}
