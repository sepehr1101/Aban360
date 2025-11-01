using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;

namespace Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Contracts
{
    public interface IMaaherService
    {
        Task<SentInvoiceRecieveDto> SendInvoice(ICollection<MaaherTSPInvoiceDto> inputDto);
    }
}
