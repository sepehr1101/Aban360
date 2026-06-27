using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts
{
    public interface IBillQueryService
    {
        Task<BillItemsGetDto> Get(int id);
        Task<IEnumerable<BillTransactionDetailGetDto>> GetBillDetails(string billId);
        Task<IEnumerable<BillHistoryDataOutputDto>> GetHistory(BillHistoryInputDto inputDto);
        Task<IEnumerable<BillLatestListDataOutputDto>> GetLatestList(BillLatestListInputDto inputDto);
    }
}
