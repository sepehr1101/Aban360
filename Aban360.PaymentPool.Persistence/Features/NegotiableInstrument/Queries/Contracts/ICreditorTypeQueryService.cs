using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts
{
    public interface ICreditorTypeQueryService
    {
        Task<CreditorType> Get(CreditorTypeEnum id);
        Task<ICollection<CreditorType>> Get();
    }
}
