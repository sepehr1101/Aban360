using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts
{
    public interface IPaymentMethodGetAllHandler
    {
        Task<ICollection<PaymentMethodGetDto>> Handle(CancellationToken cancellationToken);
    }
}
