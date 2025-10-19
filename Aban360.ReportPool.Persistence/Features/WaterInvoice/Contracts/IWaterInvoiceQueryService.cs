using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts
{
    public interface IWaterInvoiceQueryService
    {
        WaterInvoiceDto Get();
        Task<ReportOutput<WaterInvoiceDto, LineItemsDto>> Get(string billId);
        Task<int> GetOlgo(int zoneId);
    }
}
