using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts
{
    public interface IUploaderCommandService
    {
        Task Add(Uploader uploader);
        Task Remove(Uploader uploader);
    }
}
