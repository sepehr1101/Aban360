using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts
{
    public interface IBankFileStructureQueryService
    {
        Task<BankFileStructure> Get(short id);
        Task<ICollection<BankFileStructure>> Get();
    }
}
