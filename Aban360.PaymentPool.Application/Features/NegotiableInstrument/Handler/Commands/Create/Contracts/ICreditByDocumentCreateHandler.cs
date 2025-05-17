using Aban360.Common.ApplicationUser;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts
{
    public interface ICreditByDocumentCreateHandler
    {
        Task Handle(IAppUser currentUser,string? letterNumber, short bankId, Guid documentId, CancellationToken cancellationToken);
    }
}
