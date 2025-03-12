using Aban360.ReportPool.Domain.Features.Dto;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts
{
    public interface IWaterInvoiceQueryService
    {
        WaterInvoiceDto Get();
    }
}
