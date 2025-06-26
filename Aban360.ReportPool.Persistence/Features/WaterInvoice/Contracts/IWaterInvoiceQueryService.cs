using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts
{
    public interface IWaterInvoiceQueryService
    {
        WaterInvoiceDto Get();
        Task<WaterInvoiceDto> Get(string billId);
    }
}
