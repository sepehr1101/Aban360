using Aban360.Common.ApplicationUser;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts
{
    public interface ISentInvoiceHandler
    {
        Task<IEnumerable<MaaherResponseNew>> Handle(ICollection<MaaherRequestWrapper_New> inputDto, CancellationToken cancellationToken);
        Task<IEnumerable<MaaherResponseNew>> Handle(int wrapperId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
