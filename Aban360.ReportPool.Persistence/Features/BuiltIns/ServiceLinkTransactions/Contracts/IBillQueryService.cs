namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts
{
    public interface IBillQueryService
    {
        Task<bool> HasBillId(string billId);
    }
}
