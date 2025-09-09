using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts
{
    public interface IWaterInvoiceHandler
    {
        Task<ReportOutput<WaterInvoiceDto, LineItemsDto>> Handle(string input);
        WaterInvoiceDto Handle();
    }
}
