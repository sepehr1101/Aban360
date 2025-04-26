using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts
{
    public interface IAccountTypeGetSingleHandler
    {
        Task<AccountTypeGetDto> Handle(AccountTypeEnum id, CancellationToken cancellationToken);
    }
}
