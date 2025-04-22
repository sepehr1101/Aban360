using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts
{
    public interface IPaymentProcedureGetSingleHandler
    {
        Task<PaymentProcedureGetDto> Handle(PaymentProcedureEnum id, CancellationToken cancellationToken);
    }
}
