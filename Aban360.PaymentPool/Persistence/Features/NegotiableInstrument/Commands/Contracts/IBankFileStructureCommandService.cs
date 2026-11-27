using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts
{
    public interface IBankFileStructureCommandService
    {
        Task Add(BankFileStructure bankFileStructure);
        Task Remove(BankFileStructure bankFileStructure);
    }
}
