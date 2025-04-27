using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts
{
    public interface IUploaderQueryService
    {
        Task<Uploader> Get(int id);
        Task<ICollection<Uploader>> Get();
    }
}
