using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts
{
    public interface IBankGetSingleHandler
    {
        Task<BankGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
