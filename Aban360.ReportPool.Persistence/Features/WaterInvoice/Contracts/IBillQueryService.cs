using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts
{
    public interface IBillQueryService
    {
        Task<BillItemsGetDto> Get(int id);
    }
}
