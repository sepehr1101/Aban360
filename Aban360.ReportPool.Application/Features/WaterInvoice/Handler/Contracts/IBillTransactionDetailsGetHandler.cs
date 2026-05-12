using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;

namespace Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts
{
    public interface IBillTransactionDetailsGetHandler
    {
        Task<ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto>> Handle(string billId, CancellationToken cancellationToken);
    }
}
