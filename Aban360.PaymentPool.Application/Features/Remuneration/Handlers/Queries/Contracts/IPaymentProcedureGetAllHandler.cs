using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts
{
    public interface IPaymentProcedureGetAllHandler
    {
        Task<ICollection<PaymentProcedureGetDto>> Handle(CancellationToken cancellationToken);
    }
}
