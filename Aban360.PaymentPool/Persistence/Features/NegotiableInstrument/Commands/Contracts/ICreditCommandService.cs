using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts
{
    public interface ICreditCommandService
    {
        Task Add(Credit credit);
        Task Add(ICollection<Credit> credit);
        Task Remove(Credit credit);
    }
}
