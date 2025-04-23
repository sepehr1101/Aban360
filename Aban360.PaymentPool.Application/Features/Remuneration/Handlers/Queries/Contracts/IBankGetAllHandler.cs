using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts
{
    public interface IBankGetAllHandler
    {
        Task<ICollection<BankGetDto>> Handle(CancellationToken cancellationToken);
    }
}
