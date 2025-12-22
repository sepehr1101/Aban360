using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts
{
    public interface IWaterInvoiceWithLastDbQueryService
    {
        Task<ReportOutput<WaterInvoiceDto, LineItemsDto>> Get(ZoneIdAndCustomerNumberOutputDto input);
    }
}
