using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts
{
    public interface ISentInvoiceHandler
    {
        Task<SentInvoiceRecieveDto> Handle(ICollection<MaaherTSPInvoiceDto> inputDto, CancellationToken cancellationToken);
    }
}
