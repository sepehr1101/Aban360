using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts
{
    public interface IAccountTypeGetAllHandler
    {
        Task<ICollection<AccountTypeGetDto>> Handle(CancellationToken cancellationToken);
    }
}
